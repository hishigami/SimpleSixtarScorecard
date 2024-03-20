namespace SimpleSixtarScorecard;

public partial class FormMain : Form {
    private SortableBindingList<Song> songs = new(Song.SongList);
    private int dlc = 0;
    private int category = 0;

    public FormMain() {
        InitializeComponent();
        label1.Text = Profile.Instance.UserName + " ��";

        // �� ������ ��������
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.DataSource = songs;
        label2.Text = "�� " + songs.Count.ToString() + "��";

        // �޺��ڽ� ����
        comboBox1.DataSource = ((string[])(["���"])).Concat(Enum.GetValues<Dlc>().Select(dlc => dlc.ToName())).ToArray();
        comboBox2.DataSource = ((string[])(["���"])).Concat(Enum.GetValues<Category>().Select(category => category.ToString())).ToArray();
    }

    private void button1_Click(object sender, EventArgs e) {
        // ����� �̸� �ٲٱ�
        using ProfileNameDialog dialog = new(false);

        if (dialog.ShowDialog() == DialogResult.OK) {
            Profile.Instance.UserName = dialog.UserName.Trim();
            label1.Text = Profile.Instance.UserName + " ��";
        }
    }

    private void DataGridView1_SelectionChanged(object sender, EventArgs e) {
        int index;

        try {
            // ���õ� �� �ε���
            index = dataGridView1.SelectedRows[0].Index;
        } catch (ArgumentOutOfRangeException) {
            // ���õ� ���� ������ ��Ȱ��ȭ
            panel1.Visible = false;
            return;
        }

        // ���õ� ���� ������ ǥ��
        panel1.Visible = true;
        panel1.Controls.Clear();

        if (index != -1) {
            panel1.Controls.Add(new EditControl(songs[index]));
        }
    }

    private void textBox1_TextChanged(object sender, EventArgs e) => refreshSongs();

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
        dlc = comboBox1.SelectedIndex;
        refreshSongs();
    }

    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
        category = comboBox2.SelectedIndex;
        refreshSongs();
    }

    private void refreshSongs() {
        var tmpSongs = !string.IsNullOrWhiteSpace(textBox1.Text)
            ? Song.SongList.Where(song => enumTest(song) && (song.Title.Contains(textBox1.Text.Trim(), StringComparison.OrdinalIgnoreCase) || song.Composer.Contains(textBox1.Text.Trim(), StringComparison.OrdinalIgnoreCase))).ToArray()
            : Song.SongList.Where(enumTest).ToArray();

        songs = new(tmpSongs);
        dataGridView1.DataSource = songs;
        label2.Text = "�� " + songs.Count.ToString() + "��";

        bool enumTest(Song song) {
            if (dlc == 0 && category == 0) {
                // DLC�� ī�װ��� �� �� "���"���
                return true;
            } else if (dlc != 0 && category == 0) {
                // DLC�� �����Ǿ� ������
                return dlcTest();
            } else if (dlc == 0 && category != 0) {
                // ī�װ��� �����Ǿ� ������
                return categoryTest();
            } else {
                // �� �� �����Ǿ� ������
                return dlcTest() && categoryTest();
            }

            // 1�� ���� ����: �������� 0���� �����ϴµ� ���⼭�� 0�� "���"�� �����Ǿ� �ֱ� ����
            bool dlcTest() => song.Dlc == (Dlc)(dlc - 1);
            bool categoryTest() => song.Category == (Category)(category - 1);
        }
    }
}
