
// 1. CONFIGURAÇÃO DA API
const API_URL = 'http://localhost:5200/api';


// 2. NAVEGAÇÃO DO MENU (Esconder/Mostrar Telas)
function esconderTodasAsTelas() {
    document.getElementById('login').style.display = 'none';
    document.getElementById('registro').style.display = 'none';
    document.getElementById('pesquisa').style.display = 'none';
    document.getElementById('perfil').style.display = 'none';
}

// Faz os links do menu superior funcionarem
document.querySelectorAll('nav a').forEach(link => {
    link.addEventListener('click', function(evento) {
        evento.preventDefault(); 
        esconderTodasAsTelas();
        
        
        const telaDestino = this.getAttribute('href').substring(1);
        document.getElementById(telaDestino).style.display = 'block';
    });
});

// Quando o site abre pela primeira vez, mostra só o Login
window.onload = () => {
    esconderTodasAsTelas();
    document.getElementById('login').style.display = 'block';
};


// 3. LÓGICA DE LOGIN CONECTADA COM A API
const formLogin = document.getElementById('formLogin');

if (formLogin) {
    formLogin.addEventListener('submit', async function(evento) {
        evento.preventDefault(); 

        // Pega os valores das caixinhas
        const emailDigitado = document.getElementById('emailLogin').value;
        const senhaDigitada = document.getElementById('senhaLogin').value;

        try {
          
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
                
                
                localStorage.setItem('tokenEscola', dados.token);
                
      
                esconderTodasAsTelas();
                document.getElementById('registro').style.display = 'block';
            } else {
                
                alert('Erro: ' + dados.mensagem);
            }
        } catch (erro) {
            alert('Não foi possível conectar. A API C# está a rodar (dotnet run)?');
            console.error(erro);
        }
    });
}


// 4. SALVAR NOVA OCORRÊNCIA
async function salvarOcorrencia() {
   
    const data = document.getElementById('dataOcorrencia').value;
    const horario = document.getElementById('horarioOcorrencia').value;
    const materia = document.getElementById('materiaOcorrencia').value || "-";
    const professor = document.getElementById('professorOcorrencia').value || "-";
    const descricao = document.getElementById('descricaoOcorrencia').value;

  
    const checkboxes = document.querySelectorAll('input[name="infracao"]:checked');
    let infracoesSelecionadas = [];
    checkboxes.forEach((box) => {
        infracoesSelecionadas.push(box.value);
    });

    if (infracoesSelecionadas.length === 0 || !data || !horario) {
        return alert("Por favor, preencha a data, horário e marque pelo menos uma infração!");
    }

    
    const pacoteOcorrencia = {
        alunoId: 1, // ID do João Silva Sauro
        funcionarioId: 1, // ID do Diretor
        dataOcorrencia: data,
        horario: horario + ":00", 
        turmaNoMomento: "1º Ano A", 
        materia: materia,
        professorHorario: professor,
        tiposInfracao: infracoesSelecionadas.join(", "), 
        descricao: descricao,
        dataRegistroSistema: new Date().toISOString()
    };

    try {
        const resposta = await fetch(`${API_URL}/Ocorrencias`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(pacoteOcorrencia)
        });

        if (resposta.ok) {
            alert("✅ Ocorrência salva com sucesso no Banco de Dados!");
           
            document.getElementById('descricaoOcorrencia').value = '';
            checkboxes.forEach(box => box.checked = false);
        } else {
            const erro = await resposta.json();
            alert("Erro ao salvar: " + JSON.stringify(erro));
        }
    } catch (err) {
        alert("Erro de conexão. A API C# está a rodar?");
        console.error(err);
    }
}

// 5. ADICIONAR NOVO TIPO DE INFRAÇÃO NA TELA
function adicionarNovoTipoInfracao() {
    const input = document.getElementById('novoTipoTexto');
    const valor = input.value.trim(); 
    
    if (valor !== "") {
        const lista = document.getElementById('listaCheckboxes');
        
        // Cria a nova caixinha de marcação já selecionada
        const novaLabel = document.createElement('label');
        novaLabel.innerHTML = `<input type="checkbox" name="infracao" value="${valor}" checked> ${valor}`;
        
        lista.appendChild(novaLabel); 
        input.value = ""; 
    }
}