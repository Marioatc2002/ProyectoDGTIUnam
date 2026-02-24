namespace inventario.Models
{
    public class Genero
    {
        public int id{ get; set; }
        public string tipo { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }


}
