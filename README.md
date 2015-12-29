# Readme.md

I extracted this project from another project I was working on.
When I was initially setting up Nancy for the other project, I found that 
I had to read through quite a few pages from around the Internet to get 
everything that I wanted set up and running.

Here are some of the projects used:
 
 * [EntityFramework](https://msdn.microsoft.com/en-us/data/ef.aspx)
 * [MySql.Data](https://www.nuget.org/packages/MySql.Data/)
 * [MySql.Data.Entity](https://www.nuget.org/packages/MySql.Data.Entity/)
 * [MiniProfiler](https://github.com/MiniProfiler/dotnet)
 * [FluentMigrator](https://github.com/schambers/fluentmigrator)
 * [Nancy.Linker](https://github.com/horsdal/Nancy.Linker)
 * [Nancy.Authentication.Forms](https://github.com/NancyFx/Nancy/wiki/Forms-Authentication)

And some of the links I used when researching:
 * [General Notes on Nancy from an ASP.net developer](http://www.endycahyono.com/article/nancy-from-mvc-dev/)
 * [Setting up Entity Framework](http://alexboyd.me/2014/03/nancyfx-ef-crud/)
 * [Nancy.Linker info](http://www.horsdal-consult.dk/search/label/Nancy.Linker)
 * [Setting up Forms Auth with Nancy](http://www.philliphaydon.com/2012/12/18/forms-authentication-with-nancyfx/)

## Running for the first time

Copy `AppSettings.default.config` to `AppSettings.config`

Copy `Connections.default.config` to `Connections.config`

Modify both of these files with the correct values. At this point, compile your project 
to make sure everything works (you will have to compile to create the Assembly...
FluentMigrator looks for Migrations inside the assembly).

Create the database on the MySQL server, and grant your app user permissions:

    CREATE DATABASE testdb;
	CREATE USER 'your_user'@'%' IDENTIFIED BY 'your_password';
	GRANT ALL PRIVILEGES ON testdb.* TO 'your_user'@'%';
	FLUSH PRIVILEGES;

Now, run your migrations. If you are in Visual Studio, the easiest way may be to run 
them in the "Package Manager Console" (Tools -> NuGet Package Manager), since it will 
start in the solution folder. To run the migrations, use this command:

    .\packages\FluentMigrator.1.6.1\tools\Migrate.exe --target=NancySample\bin\NancySample.dll --db=MySql --conn "migrate" --configPath=NancySample\Web.config --verbose=true --task migrate

To create a test login, you can run these commands (this will set the password as "your_password" (without the quotes):

	insert into users values (1,'your_user',UUID(),'5ef8c33bb5e2c68cb87132f4cfcc812e1755af13','bbbec1aabb5659bfc10c','you@email.com');
