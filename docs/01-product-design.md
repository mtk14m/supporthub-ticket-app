# SupportHub - Product Design

## 1. Problem
SupportHub aide une equipe de support a centraliser, prioriser et suivre les demandes clients.

Sans outil dedie, les demandes arrivent par email, chat ou appels, puis se perdent facilement. Les clients ne savent pas toujours ou en est leur demande, les agents manquent de contexte, et les managers ont peu de visibilite sur la charge de travail.

La premiere version doit permettre de creer des tickets, suivre leur statut, echanger des messages, assigner un agent, et conserver un historique clair des actions importantes.

## 2. Users And Roles
### Customer

Un client qui demande de l'aide.

- Creer un ticket.
- Voir uniquement ses propres tickets.
- Ajouter des messages sur ses tickets.
- Voir le statut de ses tickets.
- Fermer un ticket si le probleme est resolu.

### SupportAgent

Un agent de support qui traite les demandes.

- Voir les tickets ouverts.
- S'assigner un ticket ou etre assigne par un manager.
- Ajouter des reponses.
- Changer le statut d'un ticket.
- Marquer un ticket comme resolu.

### SupportManager

Un responsable support qui supervise l'activite.

- Voir tous les tickets.
- Assigner ou reassigner un ticket.
- Modifier la priorite d'un ticket.
- Voir les tickets en retard.
- Suivre la charge des agents.

### Admin

Un administrateur de l'organisation.

- Gerer les utilisateurs.
- Gerer les roles.
- Configurer les categories de tickets.
- Configurer les regles de priorite et de SLA.

### System

Le systeme execute des actions automatiques.

- Envoyer des notifications.
- Detecter les tickets proches de leur SLA.
- Escalader les tickets en retard.
- Ecrire des evenements dans l'audit log.

## 3. V1 Features
La V1 reste volontairement petite. L'objectif est de construire une API propre avant d'ajouter Azure, les files de messages et l'automatisation.

1. Creer un ticket avec un titre, une description, une categorie et une priorite.
2. Consulter la liste des tickets selon le role de l'utilisateur.
3. Consulter le detail d'un ticket avec ses messages.
4. Ajouter un message a un ticket.
5. Assigner un ticket a un agent et changer son statut.

## 4. Core Entities
### User

Represente une personne qui utilise SupportHub.

- Id
- Email
- DisplayName
- Role
- IsActive
- CreatedAt

### Ticket

Represente une demande de support.

- Id
- Reference
- Title
- Description
- Status
- Priority
- CategoryId
- CustomerId
- AssignedAgentId
- CreatedAt
- UpdatedAt
- ResolvedAt
- ClosedAt

### TicketMessage

Represente un message dans la conversation d'un ticket.

- Id
- TicketId
- AuthorId
- Body
- IsInternalNote
- CreatedAt

### TicketCategory

Permet de classer les tickets.

- Id
- Name
- Description
- IsActive

### TicketAuditEvent

Garde un historique des actions importantes.

- Id
- TicketId
- ActorId
- EventType
- OldValue
- NewValue
- CreatedAt

### Attachment

Representera plus tard une piece jointe stockee dans Azure Blob Storage.

- Id
- TicketId
- UploadedById
- FileName
- ContentType
- SizeInBytes
- StoragePath
- CreatedAt

### Notification

Representera plus tard une notification email ou systeme.

- Id
- UserId
- TicketId
- Type
- Status
- CreatedAt
- SentAt

## 5. Business Rules
1. Un client ne peut voir que les tickets qu'il a crees.
2. Un agent peut voir les tickets ouverts, en cours, resolus ou assignes a lui.
3. Un manager peut voir tous les tickets de l'organisation.
4. Un ticket doit toujours avoir un titre, une description, une categorie et une priorite.
5. Un ticket nouvellement cree commence avec le statut `Open`.
6. Seul un agent ou un manager peut assigner un ticket a un agent.
7. Un ticket ne peut etre assigne qu'a un utilisateur ayant le role `SupportAgent`.
8. Un ticket `Closed` ne peut plus recevoir de message public.
9. Un ticket `Resolved` peut etre rouvert par le client si le probleme n'est pas vraiment regle.
10. Un agent ne peut pas modifier les messages d'un client.
11. Les notes internes ne sont visibles que par les agents, managers et admins.
12. Chaque changement de statut, priorite ou assignation doit creer un evenement d'audit.
13. Un ticket ne peut passer a `Resolved` que s'il contient au moins une reponse d'un agent.
14. Un utilisateur desactive ne peut plus creer de ticket ou ajouter de message.
15. Les priorites possibles en V1 sont `Low`, `Normal`, `High` et `Urgent`.

Statuts de ticket en V1:

- `Open`: le ticket vient d'etre cree.
- `InProgress`: un agent travaille dessus.
- `Resolved`: une solution a ete proposee.
- `Closed`: le ticket est termine.

## 6. Azure Services Later
### Azure App Service ou Azure Container Apps

Heberger l'API ASP.NET Core. App Service est plus simple pour commencer. Container Apps sera interessant quand on introduira des workers et une approche plus cloud-native.

### Azure Database for PostgreSQL ou Azure SQL Database

Stocker les utilisateurs, tickets, messages, categories et evenements d'audit. PostgreSQL est un excellent choix moderne pour apprendre le backend; Azure SQL est aussi tres pertinent pour les certifications Microsoft.

### Azure Blob Storage

Stocker les pieces jointes des tickets: captures d'ecran, logs, documents PDF.

### Azure Service Bus

Decoupler les actions asynchrones: envoi de notifications, escalation SLA, audit avance, traitement de pieces jointes.

### Azure Functions

Executer des jobs declenches par timer ou par message: verifier les tickets en retard, envoyer des rappels, traiter des notifications.

### Azure Key Vault

Stocker les secrets: chaines de connexion, cles d'API, secrets de signature JWT si necessaire.

### Managed Identity

Permettre a l'application d'acceder a Key Vault, Storage ou Service Bus sans stocker de mots de passe dans le code.

### Application Insights et Azure Monitor

Observer l'application: logs, traces, erreurs, performance, dependances, alertes.

### GitHub Actions

Construire les pipelines CI/CD: restore, build, test, analyse, creation d'image Docker, deploiement vers Azure.

### Bicep ou Terraform

Decrire l'infrastructure comme du code. Pour ce projet, Bicep est un bon choix car il est natif Azure et utile pour les certifications.

## 7. Technical Risks
### Authentification et autorisation

La difficulte ne sera pas seulement de connecter un utilisateur, mais de verifier correctement qui a le droit de voir ou modifier chaque ticket.

### Modelisation du domaine

Les tickets paraissent simples, mais les statuts, transitions, assignations, priorites et regles de visibilite peuvent vite devenir confus.

### Gestion des transactions

Certaines actions doivent etre coherentes: par exemple changer le statut d'un ticket et ecrire l'evenement d'audit correspondant.

### Tests d'integration

Il faudra tester l'API avec une vraie base de donnees de test pour valider les migrations, les requetes EF Core et les regles d'autorisation.

### Observabilite

En production, il faudra comprendre rapidement pourquoi une requete echoue, pourquoi un message n'est pas envoye ou pourquoi un ticket n'a pas ete escalade.

### Secrets et configuration

La configuration locale, staging et production devra etre proprement separee. Aucun secret ne doit etre commite dans Git.

### CI/CD

Le pipeline devra etre fiable: compiler, tester, publier et deployer sans manipulation manuelle fragile.

### Cout Azure

Le projet doit rester realiste financierement. On privilegiera les services gratuits ou peu couteux au debut, puis on documentera les choix plus avances.

## 8. Learning Goals

Ce projet doit servir a apprendre le backend .NET moderne et Azure de maniere progressive.

### Backend .NET

- ASP.NET Core avec .NET 10.
- API REST propre.
- Dependency Injection.
- Configuration par environnement.
- Validation des entrees.
- Gestion centralisee des erreurs.
- EF Core et migrations.
- Tests unitaires et tests d'integration.

### Azure Developer

- Deployer une API.
- Utiliser un service de base de donnees manage.
- Stocker des fichiers dans Blob Storage.
- Utiliser Service Bus pour l'asynchrone.
- Securiser les secrets avec Key Vault.
- Monitorer avec Application Insights.

### DevOps

- GitHub Flow.
- Pull requests.
- Pipelines CI.
- Pipelines CD.
- Infrastructure as Code.
- Strategies de deploiement staging/production.
- Alertes et feedback de production.
