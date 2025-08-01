@echo off
chcp 65001 > nul
echo.
echo ?? Iniciando ambiente de desenvolvimento FIAP Cloud Games...
echo.

REM Verificar se Docker est� rodando
docker info >nul 2>&1
if %errorlevel% neq 0 (
    echo ? Docker n�o est� rodando. Por favor, inicie o Docker Desktop.
    pause
    exit /b 1
)

echo ? Docker est� rodando

REM Criar diret�rios necess�rios
echo ?? Criando diret�rios necess�rios...
if not exist "logs" mkdir logs
if not exist "monitoring" mkdir monitoring
if not exist "database" mkdir database

REM Parar containers existentes
echo ?? Limpando containers antigos...
docker-compose down

REM Construir e iniciar
echo ?? Construindo e iniciando containers...
docker-compose up --build -d

REM Aguardar servi�os ficarem prontos
echo ? Aguardando servi�os iniciarem...
timeout /t 30 /nobreak > nul

REM Verificar status
echo ?? Status dos servi�os:
docker-compose ps

echo.
echo ? Ambiente pronto!
echo.
echo ???????????????????????????????????????????????????????????????
echo ?? SERVI�OS DISPON�VEIS:
echo ???????????????????????????????????????????????????????????????
echo.
echo ?? Aplica��o Principal:
echo    ?? API Base:        http://localhost:8080
echo    ?? Swagger UI:      http://localhost:8080/swagger
echo    ?? ReDoc:          http://localhost:8080/redoc
echo    ?? Health Check:   http://localhost:8080/health
echo.
echo ?? Monitoramento:
echo    ?? Grafana:        http://localhost:3000
echo       ?? Usu�rio: admin ^| ?? Senha: admin
echo    ?? Prometheus:     http://localhost:9090
echo.
echo ?? Banco de Dados:
echo    ???  SQL Server:     localhost:1433
echo       ?? Usu�rio: sa ^| ?? Senha: P@ssw0rdF1@PT3ch
echo.
echo ???????????????????????????????????????????????????????????????
echo ???  COMANDOS �TEIS:
echo ???????????????????????????????????????????????????????????????
echo.
echo ?? Ver logs da aplica��o:    docker-compose logs -f app
echo ?? Ver logs de todos:        docker-compose logs -f
echo ?? Reiniciar aplica��o:      docker-compose restart app
echo ?? Parar todos os servi�os:  docker-compose down
echo ?? Limpar volumes:           docker-compose down -v
echo.
echo ???????????????????????????????????????????????????????????????
echo.
echo ?? O ambiente est� rodando em modo desenvolvimento.
echo ?? As migra��es do banco s�o executadas automaticamente.
echo ?? Os logs s�o exibidos em tempo real nos containers.
echo.
echo ???????????????????????????????????????????????????????????????
echo ?                                                             ?
echo ?  ?? Ambiente pronto para desenvolvimento!                  ?
echo ?                                                             ?
echo ?  ? Pressione qualquer tecla para finalizar este script... ?
echo ?                                                             ?
echo ???????????????????????????????????????????????????????????????
echo.

REM Aguardar o usu�rio pressionar uma tecla
pause > nul

echo.
echo ?? Script finalizado. Os servi�os continuam rodando em background.
echo ?? Use 'docker-compose ps' para verificar o status dos containers.
echo.