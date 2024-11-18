using Questao5.Domain.Entities;

namespace Questao5Tests.Domain.Entities
{
    public class ContaCorrenteFixture
    {
        public static ContaCorrente createContaCorrenteAtiva(int numero)
        {
            return new ContaCorrente()
            {
                IdContaCorrente = "Conta_1",
                Numero = numero,
                Nome = "Nome",
                Ativo = true
            };
        }

        public static ContaCorrente createContaCorrenteInativa(int numero)
        {
            return new ContaCorrente()
            {
                IdContaCorrente = "Conta_1",
                Numero = numero,
                Nome = "Nome",
                Ativo = false
            };
        }
    }
}
