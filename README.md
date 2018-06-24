# Beer Appreciation #

The evolution of appreciating beer - made easy!

## What is this repository for?

This project started out with the aim of developing an awesome beer drinking/rating application for use by beer appreciators everywhere.
The original application was written as an asp.net MVC/Web Api application with an AngularJS front end.  
You can locate the original project under the /legacy folder.

The project has aged somewhat, and after being fucked over by the AngularJS v Angular 2x migration path, there was no enthusiasm to continue development. Plus the current implementation has a decent MVP for registering events, beverages and scoring beers.  A lot of people have requested features and enhancements but until now the source code was kept in a private repository.  I have decided it is time to open source this puppy and see where it goes.
 
## The Evolution Part

A few partakers in our regular beer drinking events have expressed an interest in using the application as a training tool for re-engineering it as a React.js application with a micro service back end.  Sounds like a great way to learn new technology and improve the beer app.

The (loose) plan is to re-implement the app as a react spa using the Visual Studio react + redux (.Net Core 2.1) template, and update it to the latest npm package versions.  The aim is to achieve (or exceed) the same level of functionality that the original application offered.

### How do I get set up?

* Clone the repository
* Restore the nuget packages
* Create a local host name record for beer-appreciation and map it to a local IIS website
* Restore the SQL database found in the /database folder.  This database contains the current production version with all user information removed (apart from their Drinking Names).
* Go for it!

### Contribution guidelines

* Lets ensure we have decent test coverage
* A pull request is required to push code into the master branch.
* Be cool.

### Who do I talk to?

If you have any questions, just get in contact.
