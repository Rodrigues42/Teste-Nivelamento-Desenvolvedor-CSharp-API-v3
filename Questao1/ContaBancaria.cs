using System;

namespace Questao1
{
    class ContaBancaria {

        public int numero { get; }

        public string titular { get; set; }

        private double saldo { get; set; }

        private double taxa = 3.5;

        public ContaBancaria(int numero, string titular, double depositoInicial = 0) { 
            
            this.numero = numero;
            this.titular = titular;
            this.saldo = depositoInicial;
        }

        public void Deposito(double quantia)
        {
            this.saldo += quantia;
        }

        public void Saque(double quantia)
        {
            var transacao = quantia + taxa;

            this.saldo -= transacao;
        }

        public override String ToString()
        {
            return $"Conta {this.numero}, Titular: {this.titular}, Saldo: $ {this.saldo:F2}";
        }
    }
}
