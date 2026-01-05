using GestaoClientes.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GestaoClientes.Domain.ValueObjects
{
    public class Cnpj
    {
        protected Cnpj() { }
        public string Valor { get; protected set; }

        private Cnpj(string valorSomenteDigitos)
        {
            Valor = valorSomenteDigitos;
        }

        public static Cnpj Criar(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new DomainException("CNPJ é obrigatório.");

            var somenteDigitos = Regex.Replace(valor, "[^0-9]", "");

            if (somenteDigitos.Length != 14)
                throw new DomainException("CNPJ deve conter 14 dígitos.");

            if (somenteDigitos.Distinct().Count() == 1)
                throw new DomainException("CNPJ inválido.");

            if (!EhValido(somenteDigitos))
                throw new DomainException("CNPJ inválido.");

            return new Cnpj(somenteDigitos);
        }

        public override string ToString() => Valor;

        private static bool EhValido(string cnpj)
        {
            int[] pesosPrimeiroDigito = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] pesosSegundoDigito = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var primeiroDigito = CalcularDigito(cnpj[..12], pesosPrimeiroDigito);
            var segundoDigito = CalcularDigito(cnpj[..12] + primeiroDigito, pesosSegundoDigito);

            return cnpj.EndsWith($"{primeiroDigito}{segundoDigito}");
        }

        private static int CalcularDigito(string baseNumerica, int[] pesos)
        {
            var soma = 0;

            for (int i = 0; i < pesos.Length; i++)
            {
                soma += (baseNumerica[i] - '0') * pesos[i];
            }

            var resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }
    }
}
