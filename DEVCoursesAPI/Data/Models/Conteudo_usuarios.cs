namespace DEVCoursesAPI.Data.Models
{
    public class Conteudo_usuarios
    {
        public Guid Id { get; set; }
        public Guid Id_treinamento { get; set; }
        public Guid Id_usuario { get; set; }
        public Guid Id_conteudo { get; set; }
        public bool Concluido { get; set; }
    }
}
