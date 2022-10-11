using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam_Troiano_Antonio
{
    abstract class Scene
    {
        public bool IsPlaying { get; set; }

        public Scene NextScene;
        public Scene LastScene;

        public Scene()
        {

        }
        public virtual void Start()
        {
            IsPlaying = true;
        }
        public virtual Scene OnExit()
        {
            IsPlaying = false;
            return NextScene;
        }

        public virtual void Update()
        {
        }
        public abstract void Input();
        public abstract void Draw();




    }
}
