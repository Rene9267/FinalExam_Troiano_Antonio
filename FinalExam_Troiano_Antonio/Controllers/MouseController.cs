using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    class MouseController : Controller
    {
        public MouseController(int controllerIndex) :base(controllerIndex)
        {

        }
        public override float GetHorizontal()
        {
            throw new NotImplementedException();
        }

        public override float GetVertical()
        {
            throw new NotImplementedException();
        }

        public override bool IsFirePressed()
        {
            throw new NotImplementedException();
        }

        public override bool IsJumpPressed()
        {
            throw new NotImplementedException();
        }
    }
}
