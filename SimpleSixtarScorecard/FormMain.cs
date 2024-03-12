namespace SimpleSixtarScorecard;

public partial class FormMain : Form {
    public FormMain() {
        InitializeComponent();
        label1.Text = Profile.Instance.UserName + " ��";

        // �� ������ ��������
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.DataSource = Song.SongList;
        label2.Text = "�� " + Song.SongList.Length.ToString() + "��";
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
            index = dataGridView1.SelectedRows[0].Index;
        } catch (ArgumentOutOfRangeException) {
            // �� �ʱ�ȭ ����
            return;
        }

        panel1.Controls.Clear();

        if (index != -1) {
            panel1.Controls.Add(new EditControl(Song.SongList[index]));
        }
    }
}
