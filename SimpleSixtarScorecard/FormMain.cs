namespace SimpleSixtarScorecard;

public partial class FormMain : Form {
    private Song[] songs = Song.SongList;

    public FormMain() {
        InitializeComponent();
        label1.Text = Profile.Instance.UserName;

        // Import song data
        dataGridView1.AutoGenerateColumns = false;
        dataGridView1.DataSource = songs;
        label2.Text = songs.Length.ToString() + " songs";
    }

    private void button1_Click(object sender, EventArgs e) {
        // Change username
        using ProfileNameDialog dialog = new(false);

        if (dialog.ShowDialog() == DialogResult.OK) {
            Profile.Instance.UserName = dialog.UserName.Trim();
            label1.Text = Profile.Instance.UserName;
        }
    }

    private void DataGridView1_SelectionChanged(object sender, EventArgs e) {
        int index;

        try {
            // Selected song index
            index = dataGridView1.SelectedRows[0].Index;
        } catch (ArgumentOutOfRangeException) {
            // Disable if no song is selected
            panel1.Visible = false;
            return;
        }

        panel1.Visible = true;
        panel1.Controls.Clear();

        if (index != -1) {
            panel1.Controls.Add(new EditControl(songs[index]));
        }
    }

    private void textBox1_TextChanged(object sender, EventArgs e) {
        songs = !string.IsNullOrWhiteSpace(textBox1.Text)
           // String search
           ? Song.SongList.Where(song => song.Title.Contains(textBox1.Text.Trim(), StringComparison.OrdinalIgnoreCase) || song.Composer.Contains(textBox1.Text.Trim(), StringComparison.OrdinalIgnoreCase)).ToArray()
           : Song.SongList;

        dataGridView1.DataSource = songs;
        label2.Text = songs.Length.ToString() + " songs";
    }
}
