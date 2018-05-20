using System;

namespace Entidades{
    [Serializable]
    public class Historias{
        public Historia[] historias;

        public Historias(Historia[] historias){
            this.historias = historias;
        }
    }
}