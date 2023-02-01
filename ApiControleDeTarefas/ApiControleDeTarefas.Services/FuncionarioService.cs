using ApiControleDeTarefas.Domain.Exceptions;
using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Repositories.Repositorio;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                _repositorio.AbrirConexao();
                ValidaCpf(model.Cpf);
                ValidaEmailNaBase(model.EmailDoFuncionario);
                ValidarModelFuncionario(model);
                model.SenhaDoFuncionario = GenerateSHA512String(model.SenhaDoFuncionario);            
                _repositorio.Inserir(model);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }

        private static void ValidarModelFuncionario(Funcionario model, bool isUpdate = false)
        {
            #region Valida Model
            if (model is null)
                throw new ValidacaoException("O json está mal formatado, ou foi enviado vazio.");
            #endregion

            #region Valida Nome

            if (model.NomeDoFuncionario.Trim().Length < 3 || model.NomeDoFuncionario.Trim().Length > 255)
                throw new ValidacaoException("O Nome do Funcionário não pode estar vazio e precisa ter entre 3 a 255 caracteres.");

            if (string.IsNullOrWhiteSpace(model.NomeDoFuncionario))
                throw new ValidacaoException("O Nome do Funcionário é obrigatório.");

            model.NomeDoFuncionario = model.NomeDoFuncionario.Trim();

            #endregion

            #region Valida Data de Admissão

            string dataDeAdmissao = model.DataDeAdmissao.ToString();
            bool isValidDataAdmissao = ValidaData(dataDeAdmissao);

            if (!isValidDataAdmissao)
                throw new ValidacaoException("A Data de Admissão informada está incorreta.");

            if (model.DataDeAdmissao > DateTime.Now.AddDays(1))
                throw new ValidacaoException("A Data de Admissão não pode ser maior que o dia atual.");
            #endregion

            #region Valida Nascimento Funcionário

            string dataNascimentoFuncionario = model.NascimentoDoFuncionario.ToString();
            bool isValidDataNascimentoFuncionario = ValidaData(dataNascimentoFuncionario);

            if (!isValidDataNascimentoFuncionario)
                throw new ValidacaoException("A Data de Nascimento do Funcionário informada está incorreta.");
            #endregion

            #region Valida Cpf

            if (!isUpdate)
            {
                if (string.IsNullOrWhiteSpace(model.Cpf))
                    throw new ValidacaoException("O Cpf do Cliente é obrigatório, gentileza informar.");

                if (!ValidarCpf(model.Cpf))
                    throw new ValidacaoException("O Cpf do Cliente é inválido, tente novamente.");
            }

            #endregion

            #region Valida Celular do Funcionário


            if (model.CelularDoFuncionario is not null
                &&
                (model.CelularDoFuncionario.Trim().Length < 11
                || model.CelularDoFuncionario.Trim().Length > 15
                || model.CelularDoFuncionario.Trim().Length != RemoverMascaraTelefone(model.CelularDoFuncionario).Length)
                )
                throw new ValidacaoException("O Celular do Funcionário tem que ter no mínimo de 11 a 15 digitos, e não pode conter mascaras.");

            
            model.CelularDoFuncionario = model.CelularDoFuncionario?.Trim();


            #endregion

            #region Valida E-mail do Funcionário

            if (model.EmailDoFuncionario.Trim().Length < 3 || model.EmailDoFuncionario.Trim().Length > 255)
                throw new ValidacaoException("O E-mail do Funcionário precisa ter entre 3 a 255 caracteres.");

            var isValidEmail = ValidaEmail(model.EmailDoFuncionario);
            if (!isValidEmail)
                throw new ValidacaoException("O E-mail informado está incorreto ou não é válido.");

            #endregion

            #region Valida Senha do Funcionário

            var isValidaSenha = ValidaSenha(model.SenhaDoFuncionario);
            if(!isValidaSenha)
            throw new ValidacaoException("A senha informada não atende aos padrões estabelecidos.");
            #endregion

            #region Valida Perfil

            int validaIntervaloPerfil = model.Perfil;
            if (!(validaIntervaloPerfil > 0 && validaIntervaloPerfil < 4))
            {
                throw new ValidacaoException("O Perfil é inválido, informe 1 - Administrador, 2 - Gerente ou 3 - Desenvolvedor.");
            }
            
            var isValidNumero = ValidaNumero(Convert.ToString(model.Perfil));
            if (!isValidNumero)
                throw new ValidacaoException("O Perfil informado não é um número informe 1 - Administrador, 2 - Gerente ou 3 - Desenvolvedor.");


               
            #endregion

        }

        #region Metódo Remove Mascara de Telefone
        private static string RemoverMascaraTelefone(string phoneNumber)
        {
            return Regex.Replace(phoneNumber, @"[^\d]", "");
        }
        #endregion

        #region Metódo Valida Cpf
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
        #endregion

        #region Metódo para Criptografia de Senhas
        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        #endregion

        #region Metódo para Criptografia de Senhas
        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        #endregion

        #region Metódo Obter Usuario por Credenciais
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
        #endregion

        #region Metódo para Criptografia de Senhas
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
        #endregion

        #region Metódo Valida E-mail
        public static bool ValidaEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        #endregion

        #region Metódo Valida Data
        public static bool ValidaData(string data)
        {
            DateTime date;
            return DateTime.TryParse(data, out date);
        }
        #endregion

        #region Metódo Valida Senha
        public static bool ValidaSenha(string senha)
        {
            // Verifique se a senha tem pelo menos 8 caracteres
            if (senha.Length < 8)
            {
                return false;
            }

            // Verifique se a senha tem pelo menos 1 letra maiúscula
            bool hasUppercase = false;
            foreach (char c in senha)
            {
                if (char.IsUpper(c))
                {
                    hasUppercase = true;
                    break;
                }
            }
            if (!hasUppercase)
            {
                return false;
            }

            // Verifique se a senha tem pelo menos 1 número
            bool hasNumber = false;
            foreach (char c in senha)
            {
                if (char.IsNumber(c))
                {
                    hasNumber = true;
                    break;
                }
            }
            if (!hasNumber)
            {
                return false;
            }

            // A senha é válida se todas as verificações passarem
            return true;
        }
        #endregion

        #region Metódo Valida Número
        public static bool ValidaNumero(string numero)
        {
            int number;
            return int.TryParse(numero, out number);
        }
        #endregion

        public void ValidaCpf(string cpf)
        {
            bool isCpfValid = _repositorio.SeExisteCpf(cpf);

            if (isCpfValid)
                throw new ValidacaoException("O Cpf informado já está cadastrado na base");
        }

        public void ValidaEmailNaBase(string email)
        {
            bool isCpfValid = _repositorio.SeExisteEmail(email);

            if (isCpfValid)
                throw new ValidacaoException("Email já cadastrado!");

        }

    }
}
