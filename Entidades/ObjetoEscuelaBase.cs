using System;

namespace CoreEscuela.Entidades
{
    public abstract class ObjetoEscuelaBase // Clase abstracta, no se puede instanciar
    {                                       // sólo heredar
        public string UniqueId { get; private set; }
        public string Nombre { get; set; }

        public ObjetoEscuelaBase()
        {
            UniqueId = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return $"{UniqueId}, {Nombre}";
        }
    }
}