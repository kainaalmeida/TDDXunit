using Bogus;
using CursoOnline.Domain.Cursos;
using CursoOnline.Domain.Test._Builders;
using CursoOnline.Domain.Test._Util;
using Moq;
using System;
using Xunit;

namespace CursoOnline.Domain.Test.Cursos
{
    public class ArmazenadorDeCursoTest
    {
        private readonly CursoDto _cursoDto;
        private readonly Mock<ICursoRepositorio> _cursoRepositorioMock;
        private readonly ArmazenadorDeCurso _armazenadorDeCurso;

        public ArmazenadorDeCursoTest()
        {

            var faker = new Faker();
            _cursoDto = new CursoDto
            {
                Nome = faker.Random.Words(),
                Descricao = faker.Lorem.Paragraph(),
                CargaHoraria = faker.Random.Double(500, 1000),
                PublicoAlvo = "Estudante",
                Valor = faker.Random.Double(100, 1000)
            };

            _cursoRepositorioMock = new Mock<ICursoRepositorio>();
            _armazenadorDeCurso = new ArmazenadorDeCurso(_cursoRepositorioMock.Object);
        }

        [Fact]
        public void DeveAdicionarCurso()
        {
            _armazenadorDeCurso.Armazenar(_cursoDto);

            _cursoRepositorioMock.Verify(r => r.Adicionar(It.Is<Curso>(c => c.Nome == _cursoDto.Nome)));
        }

        [Fact]
        public void NaoDeveAdicionarCursoComMesmoNomeDeOutroJaSalvo()
        {
            var cursoJaSalvo = CursoBuilder.Novo().ComNome(_cursoDto.Nome).Build();
            _cursoRepositorioMock.Setup(r => r.ObterPeloNome(_cursoDto.Nome)).Returns(cursoJaSalvo);

            Assert.Throws<ArgumentException>(() => _armazenadorDeCurso.Armazenar(_cursoDto))
                .ComMensagem("Nome do curso já consta no banco de dados");
        }

        [Fact]
        public void NaoDeveAdicionarPublicoAlvoInvalido()
        {
            _cursoDto.PublicoAlvo = "Medico";

            Assert.Throws<ArgumentException>(() => _armazenadorDeCurso.Armazenar(_cursoDto))
                .ComMensagem("Publico Alvo inválido");
        }
    }
}
