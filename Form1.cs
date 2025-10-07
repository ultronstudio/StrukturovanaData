using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualBasic;
using Ultron.WinForms.HelpAbout;

namespace StrukturovanaData
{
    public partial class Form1 : Form
    {
        private readonly string _csvPath;
        private DataTable _table;
        private bool _suppressAutoSave = false;
        private string _delimiter = ",";
        private int _columnContextIndex = -1; // sloupec pro context menu

        public Form1()
        {
            InitializeComponent();
            _csvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data.csv");

            this.UseStandardHelpAbout(new HelpAboutOptions
            {
                Category = TaskCategory.PraceVHodine,
                Author = "Petr Vurm",
                ExtraLines = "SPŠ Hradební — PROGRAMOVÉ VYBAVENÍ"
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                EnsureCsvExists();
                DetectDelimiter();
                LoadCsvToTable();
                BindTable();
                PopulateFilterColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při načítání dat: {ex.Message}", "Chyba",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureCsvExists()
        {
            try
            {
                if (!File.Exists(_csvPath))
                {
                    var resourceContent = Properties.Resources.Data;
                    if (string.IsNullOrWhiteSpace(resourceContent))
                        throw new InvalidOperationException("Resource Data.csv je prázdná nebo není k dispozici.");

                    File.WriteAllText(_csvPath, resourceContent, new UTF8Encoding(false));
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Nepodařilo se připravit Data.csv v output složce.", ex);
            }
        }

        private void DetectDelimiter()
        {
            try
            {
                string first = File.ReadLines(_csvPath, Encoding.UTF8).FirstOrDefault() ?? "";
                int sc = first.Count(c => c == ';');
                int cc = first.Count(c => c == ',');
                int tc = first.Count(c => c == '\t');

                if (sc >= cc && sc >= tc && sc > 0) _delimiter = ";";
                else if (cc >= sc && cc >= tc && cc > 0) _delimiter = ",";
                else if (tc > 0) _delimiter = "\t";
                else _delimiter = ",";

                toolStripStatusLabel1.Text = $"Oddělovač: {(_delimiter == "\t" ? "\\t" : _delimiter)}";
            }
            catch
            {
                _delimiter = ",";
            }
        }

        private void LoadCsvToTable()
        {
            _table = new DataTable("Data");

            try
            {
                using (var parser = new TextFieldParser(_csvPath, Encoding.UTF8))
                {
                    parser.SetDelimiters(new[] { _delimiter });
                    parser.HasFieldsEnclosedInQuotes = true;

                    // Hlavička
                    string[] headers = parser.ReadFields();
                    if (headers == null || headers.Length == 0)
                        throw new InvalidOperationException("CSV neobsahuje hlavičku.");

                    foreach (var h in headers)
                    {
                        var name = string.IsNullOrWhiteSpace(h) ? $"Sloupec{_table.Columns.Count + 1}" : h.Trim();
                        if (_table.Columns.Contains(name))
                        {
                            int i = 2;
                            var baseName = name;
                            while (_table.Columns.Contains(name)) name = $"{baseName}_{i++}";
                        }
                        _table.Columns.Add(name);
                    }

                    // Data
                    while (!parser.EndOfData)
                    {
                        var fields = parser.ReadFields() ?? Array.Empty<string>();
                        if (fields.Length < _table.Columns.Count)
                            fields = fields.Concat(Enumerable.Repeat(string.Empty, _table.Columns.Count - fields.Length)).ToArray();
                        if (fields.Length > _table.Columns.Count)
                            fields = fields.Take(_table.Columns.Count).ToArray();

                        _table.Rows.Add(fields);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Chyba při čtení CSV souboru.", ex);
            }
        }

        private void BindTable()
        {
            try
            {
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.AllowUserToAddRows = true;    // psaní do * řádku = nový záznam
                dataGridView1.AllowUserToOrderColumns = true;
                dataGridView1.DataSource = _table;

                dataGridView1.DataError += (_, __) => { /* potlač drobné konverzní chyby */ };

                // Auto-save na změny
                dataGridView1.CellValueChanged += (_, __) => SafeAutoSave();
                dataGridView1.RowValidated += (_, __) => SafeAutoSave();
                dataGridView1.UserAddedRow += (_, __) => SafeAutoSave();
                dataGridView1.UserDeletedRow += (_, __) => SafeAutoSave();

                // Context menu na hlavičce
                dataGridView1.ColumnHeaderMouseClick += DataGridView1_ColumnHeaderMouseClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při vazbě dat: {ex.Message}", "Chyba",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateFilterColumns()
        {
            try
            {
                cboFilterColumn.Items.Clear();
                foreach (DataColumn col in _table.Columns)
                    cboFilterColumn.Items.Add(col.ColumnName);

                if (cboFilterColumn.Items.Count > 0)
                    cboFilterColumn.SelectedIndex = 0;

                if (cboFilterMode.Items.Count == 0)
                {
                    cboFilterMode.Items.Add("obsahuje");
                    cboFilterMode.Items.Add("neobsahuje");
                    cboFilterMode.Items.Add("přesně");
                    cboFilterMode.SelectedIndex = 0;
                }
            }
            catch { /* ignore */ }
        }

        private void SafeAutoSave()
        {
            try
            {
                if (_suppressAutoSave) return;
                SaveTableToCsv();
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = $"Uložení selhalo: {ex.Message}";
            }
        }

        private void SaveTableToCsv()
        {
            try
            {
                _suppressAutoSave = true;

                // commit rozeditovaných buněk
                try
                {
                    dataGridView1.EndEdit();
                    var cm = this.BindingContext[dataGridView1.DataSource] as CurrencyManager;
                    cm?.EndCurrentEdit();
                }
                catch { /* ignore */ }

                var sb = new StringBuilder();

                // Hlavička
                sb.AppendLine(string.Join(_delimiter,
                    _table.Columns.Cast<DataColumn>().Select(c => CsvEscape(c.ColumnName, _delimiter))));

                // Data
                foreach (DataRow row in _table.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;
                    var fields = _table.Columns.Cast<DataColumn>()
                        .Select(c => CsvEscape(Convert.ToString(row[c] ?? string.Empty), _delimiter));
                    sb.AppendLine(string.Join(_delimiter, fields));
                }

                File.WriteAllText(_csvPath, sb.ToString(), new UTF8Encoding(false));
                toolStripStatusLabel1.Text = $"Uloženo: {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                throw new IOException("Nepodařilo se zapsat do CSV souboru.", ex);
            }
            finally
            {
                _suppressAutoSave = false;
            }
        }

        private static string CsvEscape(string input, string delimiter)
        {
            if (input == null) return "";
            char d = delimiter == "\t" ? '\t' : delimiter[0];
            bool mustQuote = input.Contains(d) || input.Contains("\"") || input.Contains("\n") || input.Contains("\r");
            string s = input.Replace("\"", "\"\"");
            return mustQuote ? $"\"{s}\"" : s;
        }

        // ----------------- FILTRACE -----------------
        private void btnFilterApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (_table == null) return;
                string col = cboFilterColumn.SelectedItem?.ToString();
                string mode = cboFilterMode.SelectedItem?.ToString();
                string val = txtFilterValue.Text ?? "";

                if (string.IsNullOrWhiteSpace(col))
                {
                    MessageBox.Show("Vyber sloupec pro filtrování.", "Filtr", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Escapy pro DataColumn název a LIKE pattern
                string colExpr = EscapeColumnName(col);
                string value = EscapeLikeValue(val);

                string filter;
                switch (mode)
                {
                    case "neobsahuje":
                        filter = $"{colExpr} NOT LIKE '%{value}%'";
                        break;
                    case "přesně":
                        // přesně – použij rovnost (jednoduché u stringů)
                        string valueEq = val.Replace("'", "''");
                        filter = $"{colExpr} = '{valueEq}'";
                        break;
                    case "obsahuje":
                    default:
                        filter = $"{colExpr} LIKE '%{value}%'";
                        break;
                }

                _table.DefaultView.RowFilter = filter;
                toolStripStatusLabel1.Text = "Filtr aplikován.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba filtrace: {ex.Message}", "Filtr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFilterClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (_table == null) return;
                _table.DefaultView.RowFilter = string.Empty;
                txtFilterValue.Clear();
                toolStripStatusLabel1.Text = "Filtr zrušen.";
            }
            catch { /* ignore */ }
        }

        // Escapuje název sloupce pro DataView.RowFilter (hranaté závorky)
        private static string EscapeColumnName(string name)
        {
            // [ je třeba escapovat jako [[] , ] uvnitř názvu jako ]]
            return "[" + name.Replace("[", "[[").Replace("]", "]]") + "]";
        }

        // Escapuje hodnotu pro LIKE (%, _, [, ' )
        private static string EscapeLikeValue(string value)
        {
            if (value == null) return "";
            return value
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("_", "[_]");
        }

        // ----------------- CONTEXT MENU NA HLAVIČCE -----------------
        private void DataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right && e.ColumnIndex >= 0)
                {
                    _columnContextIndex = e.ColumnIndex;
                    cmsColumn.Show(Cursor.Position);
                }
            }
            catch { /* ignore */ }
        }

        private void miDeleteColumn_Click(object sender, EventArgs e)
        {
            try
            {
                if (_columnContextIndex < 0 || _columnContextIndex >= _table.Columns.Count) return;

                if (_table.Columns.Count == 1)
                {
                    MessageBox.Show("Nelze smazat poslední sloupec.", "Smazat sloupec", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string colName = _table.Columns[_columnContextIndex].ColumnName;
                var confirm = MessageBox.Show($"Opravdu smazat sloupec „{colName}“?\nTato akce je nevratná (v CSV).",
                    "Potvrdit smazání", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                _table.Columns.RemoveAt(_columnContextIndex);
                PopulateFilterColumns(); // refresh výběru pro filtr
                SafeAutoSave();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Smazání sloupce selhalo: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { _columnContextIndex = -1; }
        }

        private void miInsertBefore_Click(object sender, EventArgs e)
        {
            InsertColumn(relativeOffset: 0);
        }

        private void miInsertAfter_Click(object sender, EventArgs e)
        {
            InsertColumn(relativeOffset: 1);
        }

        private void InsertColumn(int relativeOffset)
        {
            try
            {
                if (_columnContextIndex < 0) return;
                int insertAt = Math.Max(0, Math.Min(_columnContextIndex + relativeOffset, _table.Columns.Count));

                string ask = Interaction.InputBox("Název nového sloupce:", "Vložit sloupec", $"Sloupec{_table.Columns.Count + 1}");
                if (string.IsNullOrWhiteSpace(ask)) return;
                string name = ask.Trim();

                // ošetři duplicitu
                string final = name;
                if (_table.Columns.Contains(final))
                {
                    int i = 2;
                    while (_table.Columns.Contains(final)) final = $"{name}_{i++}";
                }

                // přidej a posuň na správné místo
                _table.Columns.Add(final, typeof(string));
                _table.Columns[final].SetOrdinal(insertAt);

                foreach (DataRow r in _table.Rows)
                    if (r.RowState != DataRowState.Deleted) r[final] = string.Empty;

                PopulateFilterColumns();
                // vyber první buňku v novém sloupci
                int colIndex = _table.Columns.IndexOf(final);
                if (colIndex >= 0 && dataGridView1.Rows.Count > 0)
                    dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[colIndex];

                SafeAutoSave();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Vložení sloupce selhalo: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { _columnContextIndex = -1; }
        }

        // ----------------- EXISTUJÍCÍ TLAČÍTKA -----------------
        private void btnAddColumn_Click(object sender, EventArgs e)
        {
            try
            {
                if (_table == null)
                {
                    MessageBox.Show("Tabulka není inicializovaná.", "Varování",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string name = Interaction.InputBox("Zadej název nového sloupce:", "Přidat sloupec", $"Sloupec{_table.Columns.Count + 1}");
                if (string.IsNullOrWhiteSpace(name)) return;
                name = name.Trim();

                string final = name;
                if (_table.Columns.Contains(final))
                {
                    int i = 2;
                    while (_table.Columns.Contains(final)) final = $"{name}_{i++}";
                }

                _table.Columns.Add(final, typeof(string));
                foreach (DataRow r in _table.Rows)
                    if (r.RowState != DataRowState.Deleted) r[final] = string.Empty;

                int colIndex = _table.Columns.IndexOf(final);
                if (colIndex >= 0 && dataGridView1.Rows.Count > 0)
                    dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[colIndex];

                PopulateFilterColumns();
                SafeAutoSave();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při přidávání sloupce: {ex.Message}", "Chyba",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            try
            {
                DetectDelimiter();
                LoadCsvToTable();
                BindTable();
                PopulateFilterColumns();
                toolStripStatusLabel1.Text = "Načteno z disku.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při opětovném načtení: {ex.Message}", "Chyba",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveTableToCsv();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při ručním uložení: {ex.Message}", "Chyba",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
