using ApiControleDeTarefas.Domain.Exceptions;
using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Repositories.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Services
{
    public class FuncionarioService
    {
        private readonly FuncionarioRepositorio _repositorio;
        public FuncionarioService(FuncionarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public List<Funcionario> Listar(string? nomeDoFuncionario)
        {
            try
            {
                _repositorio.AbrirConexao();
                return _repositorio.ListarFuncionarios(nomeDoFuncionario);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public Funcionario Obter(int funcionarioId)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.SeExiste(funcionarioId);
                return _repositorio.Obter(funcionarioId);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Atualizar(Funcionario model)
        {
            try
            {
                ValidarModelFuncionario(model);
                _repositorio.AbrirConexao();
                _repositorio.Atualizar(model);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Deletar(int FuncionarioId)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.Deletar(FuncionarioId);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Inserir(Funcionario model)
        {
            try
            {
                var novaSenha = GenerateSHA512String(model.SenhaDoFuncionario);
                model.SenhaDoFuncionario = novaSenha;
                ValidarModelFuncionario(model);
                _repositorio.AbrirConexao();
                _repositorio.Inserir(model);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        private static void ValidarModelFuncionario(Funcionario model, bool isUpdate = false)
        {
            if (model is null)
                throw new ValidacaoException("O json está mal formatado, ou foi enviado vazio.");

            if (model.NomeDoFuncionario.Trim().Length < 3 || model.NomeDoFuncionario.Trim().Length > 255)
                throw new ValidacaoException("O Nome do Funcionário não pode estar vazio e precisa ter entre 3 a 255 caracteres.");

            if (model.EmailDoFuncionario.Trim().Length < 3 || model.EmailDoFuncionario.Trim().Length > 255)
                throw new ValidacaoException("O E-mail do Funcionário precisa ter entre 3 a 255 caracteres.");

            if (!isUpdate)
            {
                if (string.IsNullOrWhiteSpace(model.Cpf))
                    throw new ValidacaoException("O Cpf do Cliente é obrigatório, gentileza informar.");

                if (!ValidarCpf(model.Cpf))
                    throw new ValidacaoException("O Cpf do Cliente é inválido, tente novamente.");
            }

            if (string.IsNullOrWhiteSpace(model.NomeDoFuncionario))
                throw new ValidacaoException("O Nome do Funcionário é obrigatório.");

            if (model.CelularDoFuncionario is not null
                &&
                (model.CelularDoFuncionario.Trim().Length < 11
                || model.CelularDoFuncionario.Trim().Length > 15
                || model.CelularDoFuncionario.Trim().Length != RemoverMascaraTelefone(model.CelularDoFuncionario).Length)
                )
                throw new ValidacaoException("O Celular do Funcionário tem que ter no mínimo de 11 a 15 digitos, e não pode conter mascaras.");

            model.NomeDoFuncionario = model.NomeDoFuncionario.Trim();
            model.CelularDoFuncionario = model.CelularDoFuncionario?.Trim();

        }
        private static string RemoverMascaraTelefone(string phoneNumber)
        {
            return Regex.Replace(phoneNumber, @"[^\d]", "");
        }
        private static bool ValidarCpf(string? cpf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cpf))
                    return false;

                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                string tempCpf;
                string digito;
                int soma;
                int resto;
                cpf = cpf.Trim();
                cpf = cpf.Replace(".", "").Replace("-", "");
                if (cpf.Length != 11)
                    return false;
                tempCpf = cpf.Substring(0, 9);
                soma = 0;

                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = resto.ToString();
                tempCpf = tempCpf + digito;
                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = digito + resto.ToString();
                return cpf.EndsWith(digito);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        public Funcionario? ObterUsuarioPorCredenciais(string email, string senha, bool isDescriptografado = true)
        {
            try
            {
                _repositorio.AbrirConexao();

                if (isDescriptografado)
                    senha = CriptografarSha512(senha);

                return _repositorio.ObterFuncionarioPorCredenciais(email, senha);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }

        private static string CriptografarSha512(string texto)
        {
            var bytes = Encoding.UTF8.GetBytes(texto);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString().ToLower();
            }
        }


    }
}
