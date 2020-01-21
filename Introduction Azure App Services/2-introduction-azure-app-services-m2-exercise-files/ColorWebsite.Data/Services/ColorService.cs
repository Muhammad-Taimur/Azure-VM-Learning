using ColorWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorWebsite.Data.Services
{
    public class ColorService
    {
        private KnownColor[] _names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
        

        public List<DemoColor> GetAllColors()
        {
            List<DemoColor> result = new List<DemoColor>();

            using(var dc = new DataContext())
            {
                result = dc.Colors.ToList();
            }

            return result;
        }

        public void DeleteColors()
        {
            using (var dc = new DataContext())
            {
                foreach (var color in dc.Colors.ToList())
                {
                    dc.Colors.Remove(color);                   
                }

                dc.SaveChanges();
            }
        }

        public void InsertRandomColors()
        {
            Random random = new Random();

            Color color1 = GetRandomColor(random.Next(_names.Length));
            Color color2 = GetRandomColor(random.Next(_names.Length));
            Color color3 = GetRandomColor(random.Next(_names.Length));

            using (var dc = new DataContext())
            {
                dc.Colors.Add(new DemoColor { Name = color1.Name });
                dc.Colors.Add(new DemoColor { Name = color2.Name });
                dc.Colors.Add(new DemoColor { Name = color3.Name });
                dc.SaveChanges();
            }
        }

        private Color GetRandomColor(int indexOfColor)
        {
            KnownColor randomColorName = _names[indexOfColor];
            Color randomColor = Color.FromKnownColor(randomColorName);

            return randomColor;
        }
    }
}
