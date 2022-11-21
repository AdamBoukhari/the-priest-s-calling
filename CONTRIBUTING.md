# Contribuer

Veuillez noter que nous avons un code de conduite. Vous devez le suivre durant toute votre implication dans le projet.

## Code de conduite

Dans l'intérêt de favoriser un environnement ouvert et accueillant, nous nous engageons, en tant que contributeurs 
et responsables de ce projet, à faire de la participation à notre projet une expérience agréable pour tous, quel 
que soit le niveau d'expérience. Nous comprenons que des erreurs peuvent survenir pour diverses raisons et nous 
engageons à maintenir un comportement professionnel à tout moment.

Exemples de comportements qui contribuent à créer un environnement positif :

* Être respectueux des différents points de vue et expériences
* Accepter la critique lorsqu'elle est constructive
* Toujours agir dans le meilleur intérêt du projet

Exemples de comportements non acceptables :

* L'utilisation d'un langage ou d'imagerie non professionnelle
* Trolling, commentaires insultants et attaques personnelles
* Tout autre comportement qui pourrait être considéré comme inapproprié dans un cadre professionnel

Tout contributeur au projet a le droit et la responsabilité de supprimer, modifier ou rejeter toute contribution qui
ne respectent pas le code de conduite.

Ce code de conduite s'applique à la fois au sein des espaces du projet ainsi que dans les espaces publics 
(par exemple, une réunion *dans la vie réelle*). Ceci s'applique également lorsqu'un individu représente le projet. 
Font parties des exemples de représentation d'un projet le fait d'utiliser une adresse email propre au projet, de
poster sur les réseaux sociaux avec un compte officiel, ou d'intervenir comme représentant à un événement en-ligne 
ou hors-ligne. La représentation du projet pourra être autrement définie et clarifiée par les mainteneurs du
projet.

Les contributeurs qui ne respectent pas le code de conduite de bonne foi peuvent faire face temporairement ou 
définitivement à des répercussions telles que l'expulsion du projet ou toute autre conséquence déterminée par les 
autres membres du projet.

## Procédure de requête de fusion
  
1. Assurez-vous qu'aucun fichier temporaire ne se retrouve sur le dépôt.
1. Assurez-vous que le projet fonctionne dans l'éditeur ainsi qu'en mode autonome.
1. Créez la requête de fusion.
1. Fusionnez la requête de fusion une fois que vous avez obtenu l'approbation de deux autres développeurs.

## Convention de nommage des branches

Lorsque vous êtes prêt à réaliser un changement, créez une branche à partir de la dernière révision de la 
branche `develop`. Le nom des branches doit suivre la nomenclature décrite ci-dessous.

| Type de changement | Préfix de la branche | Exemple                               |
| ------------------ |----------------------| ------------------------------------- |
| Fonctionalité      | `feature`            | `feature/854-main-menu`               |
| Bogue non urgent   | `bug`                | `bug/421-cant-jump-off-ladder`        |
| Bogue urgent       | `hotfix`             | `hotfix/687-cant-win-tutorial-level`  |
| Corvée             | `chore`              | `chore/22-add-missing-tooltips`       |

## Écriture de messages de *Commit*

Pour des questions d'historique, faites des messages de *Commit* détaillant ce que contient la révision. Nul besoin 
d'être très exhaustif : juste mentionner les changements en général. En général, il est beaucoup plus facile de 
faire un bon message de *Commit* si ce dernier est petit. Une bonne habitude à prendre est donc de faire des 
petites révisions.