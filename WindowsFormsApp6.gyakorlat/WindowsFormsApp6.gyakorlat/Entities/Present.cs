using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.gyakorlat.Abstractions;

namespace WindowsFormsApp6.gyakorlat.Entities
{
    class Present : Toy
    {
        public Present(Color ribbon, Color box)
        {

        }

        protected override void DrawImage(Graphics g)
        {
            throw new NotImplementedException();
        }
    }
}
