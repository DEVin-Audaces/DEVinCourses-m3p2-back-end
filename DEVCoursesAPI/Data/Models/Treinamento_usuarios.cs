namespace DEVCoursesAPI.Data.Models
{
    public class Treinamento_usuarios
    {
        public Guid Id { get; set; }
        public Guid Id_treinamento { get; set; }
        public Guid Id_usuario { get; set; }
        public bool Concluido { get; set; }
        public DateTime Data_matricula { get; set; }
    }
}