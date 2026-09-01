### Aplicaciones Distribuidas - Microservicios con RabbitMQ y API Gateway

Este proyecto implementa una arquitectura orientada a microservicios utilizando contenedores. 
Demuestra la comunicación síncrona a través de un API Gateway y la comunicación asíncrona basada en eventos 
mediante un bus de mensajes.

### Tecnologías Utilizadas
* ASP.NET Core Web API
* RabbitMQ (Mensajería asíncrona)
* Docker Compose (Orquestación de contenedores)
* API Gateway (YARP - Yet Another Reverse Proxy)
* Microsoft SQL Server
* Swagger (Documentación de APIs)

### Estructura del Proyecto
La estructura principal incluye las carpetas para el API Gateway, 
los microservicios de Libro, Inventario, Categoría y Vehículo, 
además de los archivos de configuración de Docker.

/
├── ApiGetewayA/
├── CategoriaA/
├── InventarioA/
├── LibroA/
├── VehiculoA/
├── BaseDatos/
│   ├── CategoriaDBA.sql
│   ├── InventarioDBA.sql
│   ├── LibroDBA.sql
│   └── VehiculoDBA.sql
├── docker-compose.yml
├── .gitignore
└── README.md

Los scripts SQL (CategoriaDBA.sql, InventarioDBA.sql, LibroDBA.sql, VehiculoDBA.sql) 
están dentro de la carpeta BaseDatos. Estos archivos contienen los esquemas, tablas, datos iniciales y 
las credenciales de usuarios requeridas por las cadenas de conexión (ConnectionStrings) de cada microservicio.

### Funcionamiento General
El sistema centraliza las peticiones externas a través del API Gateway. 
El flujo general inicia en el Cliente, pasa por el API Gateway, interactúa con el Microservicio correspondiente 
(ej. Libro), publica un evento en RabbitMQ y es consumido por otro Microservicio (ej. Inventario) 
que interactúa con SQL Server.

Cliente
   ↓
API Gateway :5080
   ↓
Microservicios (Libro :5001 | Categoria :5003 | Vehiculo :5004)
   ↓
RabbitMQ :15672
   ↓
Microservicio Suscriptor (Ej. Inventario :5002)
   ↓
SQL Server (Anfitrión)


### Guía de Ejecución y Pruebas
1. Preparar las Bases de Datos
Antes de levantar los contenedores, asegúrate de ejecutar los 4 scripts ubicados en la 
carpeta BaseDatos en tu motor local de SQL Server. Esto creará los usuarios y la estructura necesaria.

2. Levantar la Infraestructura
Abre una terminal en la raíz del proyecto (donde se encuentra el archivo docker-compose.yml) 
y ejecuta el siguiente comando para construir las imágenes e iniciar todos los servicios integrados:

```Bash
docker compose up --build
```

3. Probar los Servicios
Cuando los registros de la terminal indiquen que las aplicaciones están escuchando en el puerto 8080 
(interno del contenedor), puedes comenzar a consumir las APIs.

# Pruebas unificadas vía API Gateway:
Realiza las peticiones HTTP apuntando exclusivamente al puerto 5080. 
El Gateway enrutará automáticamente el tráfico:

```Bash
* http://localhost:5080/api/libros
```

```Bash
* http://localhost:5080/api/inventarios
```

```Bash
* http://localhost:5080/api/categoria
```

```Bash
* http://localhost:5080/api/vehiculo
```

# Pruebas directas con interfaz gráfica (Swagger):
Puedes acceder a la documentación interactiva de cada microservicio de manera independiente:

```Bash
* Microservicio Libro: http://localhost:5001/swagger
```

```Bash
* Microservicio Inventario: http://localhost:5002/swagger
```

```Bash
* Microservicio Categoría: http://localhost:5003/swagger
```

```Bash
* Microservicio Vehículo: http://localhost:5004/swagger
```

# Monitoreo de Mensajería (RabbitMQ):
Para visualizar las colas de mensajes (libro_creado, categoria_creado), ingresa al panel de administración:

```Bash
* URL: http://localhost:15672

* Usuario: admin

* Contraseña: admin123
```