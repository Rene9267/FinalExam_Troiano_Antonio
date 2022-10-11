using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    interface IDrawable
    {
        DrawLayer Layer { get; }
        void Draw();
    }
}
