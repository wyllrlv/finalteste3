using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace puc_projeto_eixo_2.Models
{
    [Table("Table_Treino")]
    public class Treino
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatorio informar o titulo")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Obrigatorio informar a descrição")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Avaliação")]
        [Range(0, 5, ErrorMessage = "A avaliação deve ser um número entre 0 e 5.")]
        public int Avaliacao { get; set; }

        public List<ExercicioModel> Exercicios { get; set; } = new List<ExercicioModel>();

        public List<ComentarioModel> Comentarios { get; set; } = new List<ComentarioModel>();

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Obrigatorio informar o Usuário")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
    }

    public class ExercicioModel
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }

        [ForeignKey("TreinoId")]
        public int TreinoId { get; set; }
    }

    public class ComentarioModel
    {
        [Key]
        public int Id { get; set; }
        public string Texto { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Usuario { get; set; }

        [ForeignKey("TreinoId")]
        public int TreinoId { get; set; }
    }
}

