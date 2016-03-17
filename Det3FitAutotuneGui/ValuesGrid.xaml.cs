using System.Windows;
using System.Windows.Controls;

namespace Det3FitAutotuneGui
{
    /// <summary>
    /// Interaction logic for ValuesGrid.xaml
    /// </summary>
    public partial class ValuesGrid : UserControl
    {
        public ValuesGrid()
        {
            InitializeComponent();
        }

        public void Populate()
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    var txt = new TextBlock
                    {
                        Name = string.Format("val{0}_{1}", i, j),
                        Text = string.Format("val{0}_{1}", i, j),
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    TheGrid.Children.Add(txt);
                    Grid.SetColumn(txt, i);
                    Grid.SetRow(txt, j);
                }
            }
        }
    }
}
