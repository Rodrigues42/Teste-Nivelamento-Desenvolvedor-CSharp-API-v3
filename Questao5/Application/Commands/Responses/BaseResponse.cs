namespace Questao5.Application.Commands.Responses
{
    public class BaseResponse
    {
        public bool Sucesso { get; set; } =  true;
        public string Mensagem { get; set; } = "Ok";
        public string TipoErro { get; set; } = "";
    }
}
