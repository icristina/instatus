Instatus
========

Library for content managed microsites, competitions and Facebook applications.

Features
--------

* Entity model for pages, places, products, offers, activities, highscores, achievements and more
* Scaffold controllers for DbContext
* Scaffold views for list views, pagination and navigation
* Custom DisplayTemplates and EditorTemplates for ASP.NET MVC 3
* Twitter Boostrap themed admin views
* Facebook authentication and graph helpers
* Twitter and Google Analytics helpers
* Generator class for realistic seed data
* Many HtmlHelper and utility extension methods

Technology
----------

* ASP.NET MVC 3
* Entity Framework 4.1 Code First
* Modernizr, jQuery, LessJs, RespondJs
* Twitter Bootstrap

Instatus Roadmap
----------------

0.6 current

0.7

* when: pending
* upgrade to EF 4.3, testing code based alternative to DDL scripts
* replace LogErrorAttribute with HealthMonitoring buffered implementation
* make as many controllers sessionless as possible, including removing all TempData calls
* global CacheDependency as alternative to MessageBus
* review Instatus.js code
* review MicrositeArea PageController and whether still required

0.8 

* when: ASP.NET MVC 4 and Web API RTM
* update ExtendedModelMetaData provider for MVC 4 html5 input type improvements
* replace Html.OptionalAttribute with Razor 2 syntax
* review Restrictions api for access control and competition logic

0.9

* when: intermediate release
* replace WordpressArea with Web API implementation
* replace Facebook authentication endpoints with Web API implementation
* evaluate Web API endpoints for javascript and Flash client side code, including logging, auth and activity endpoints eg. highscores
* evaluate client side Web API for Facebook and Twitter Http Requests, with better response code handling

1.0

* when: ASP.NET 4.5 and EF 5.0 RTM
* replace MefDependencyResolver with System.ComponentModel.Composition.Ioc implementation
* OAuth integration
* review EmbeddedVirtualPathProvider and use of embedded views 
* update Model and queries with Enum support from EF 5.0
* add StringLength and other detailed constraints to Model