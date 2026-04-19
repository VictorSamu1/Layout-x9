// 1. Defina o endereço da sua API C#
const API_URL = 'http://localhost:5000/api';

// ==========================================
// NAVEGAÇÃO DO MENU (Esconder/Mostrar Telas)
// ==========================================
function esconderTodasAsTelas() {
    document.getElementById('login').style.display = 'none';
    document.getElementById('registro').style.display = 'none';
    document.getElementById('pesquisa').style.display = 'none';
    document.getElementById('perfil').style.display = 'none';
}

// Faz os links do menu superior funcionarem
document.querySelectorAll('nav a').forEach(link => {
    link.addEventListener('click', function(evento) {
        evento.preventDefault(); // Evita que a página pisque
        esconderTodasAsTelas();
        
        // Descobre qual tela o link quer abrir (ex: tira o # de '#registro')
        const telaDestino = this.getAttribute('href').substring(1);
        document.getElementById(telaDestino).style.display = 'block';
    });
});

// Quando o site abre pela primeira vez, mostra só o Login
window.onload = () => {
    esconderTodasAsTelas();
    document.getElementById('login').style.display = 'block';
};

// ==========================================
// LÓGICA DE LOGIN CONECTADA COM A API
// ==========================================
const formLogin = document.getElementById('formLogin');

if (formLogin) {
    formLogin.addEventListener('submit', async function(evento) {
        evento.preventDefault(); // Não recarrega a página ao clicar em "Entrar"

        // Pega os valores das caixinhas
        const emailDigitado = document.getElementById('emailLogin').value;
        const senhaDigitada = document.getElementById('senhaLogin').value;

        try {
            // Manda o pedido para o C#
            const resposta = await fetch(`${API_URL}/Auth/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ 
                    email: emailDigitado, 
                    senha: senhaDigitada 
                })
            });

            const dados = await resposta.json();

            if (resposta.ok) {
                // Sucesso!
                alert('Bem-vindo(a) ao sistema, ' + dados.nome + '!');
                
                // Salva o "Crachá" (Token JWT) no navegador
                localStorage.setItem('tokenEscola', dados.token);
                
                // Muda automaticamente para a tela de Registro
                esconderTodasAsTelas();
                document.getElementById('registro').style.display = 'block';
            } else {
                // Erro (senha errada, usuário não existe)
                alert('Erro: ' + dados.mensagem);
            }
        } catch (erro) {
            alert('Não foi possível conectar. A API C# está rodando (dotnet run)?');
            console.error(erro);
        }
    });
}