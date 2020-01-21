using ColorWebsite.Api.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ColorWebsite.Api.Services
{
    public class ColorService
    {
        private KnownColor[] _names = (KnownColor[])Enum.GetValues(typeof(KnownColor));


        public List<DemoColor> GetAllColors()
        {
            List<DemoColor> result = new List<DemoColor>();

            using (var dc = new DataContext())
            {
                result = dc.Colors.ToList();
            }

            return result;
        }

        public IEnumerable<string> GetAllColornames()
        {
            IEnumerable<string> result;

            using (var dc = new DataContext())
            {
                result = dc.Colors.Select(x => x.Name).ToList(); 
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
                dc.Colors.Add(new DemoColor { Name = color1.Name, Id = Guid.NewGuid().ToString() });
                dc.Colors.Add(new DemoColor { Name = color2.Name, Id = Guid.NewGuid().ToString() });
                dc.Colors.Add(new DemoColor { Name = color3.Name, Id = Guid.NewGuid().ToString() });
                dc.SaveChanges();
            }
        }

        private Color GetRandomColor(int indexOfColor)
        {
            KnownColor randomColorName = _names[indexOfColor];
            Color randomColor = Color.FromKnownColor(randomColorName);

            return randomColor;
        }

        public string ChangeColors(Color color1, Color color2, Color color3)
        {
            string message = string.Empty;

            if (color1 != null && color2 != null && color3 != null)
            {
                DeleteColors();

                using (var dc = new DataContext())
                {
                    dc.Colors.Add(new DemoColor { Name = color1.Name });
                    dc.Colors.Add(new DemoColor { Name = color2.Name });
                    dc.Colors.Add(new DemoColor { Name = color3.Name });
                    dc.SaveChanges();
                }
            }
            else
            {
                message = "You must provide all 3 colors!";
            }

            return message;
        }
    }
}