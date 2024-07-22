# WoodstocksIMS 

## by Darren Gansberg (C) 2014

### Project Description 

The Woodstocks Inventory Management System (WoodstocksIMS) is a .NET WinForms based inventory management program created for a fictional wooden toy retailer Wood Stocks. 

The application is created as a means of demonstrating knowledge, skills and abilities to create a functional program with the C# programming language, targeting the .NET Framework.

The Wood Stocks retailer sells wooden toys. 

The WoodstocksIMS has been created to support Wood Stocks daily stock control operations, and allows members of its organisation, in particular its Office Administrator(s) to:
•	Import data relating to its toy stock to enable the viewing of details for its stock that include the item code, item description, current count and on order status for a toy item.
•	Update the current count of toy stock carried by the business, each day.
•	Sort toy data according to item code, current count and on order status. 
•	Export changes of current count to an updated stock data file.

A final release build can be found in the \distribution folder.

At a later date this application may be modified to allow the user to select the location of the stock data file. 
However, for now, please ensure stocklist.csv file is loaded into C:\StockFile prior to running the application.

### Developer Documentation 

The developer documentation for the application was built using Sandcastle Help Builder. 

The documentation for the application can be found in” \documents\developer documentation”. 

There are three identical files included in compiled help file (chm), pdf and docx format for convenience.

### Design Documentation 

Contained within the design folder (\documents\design) are two files containing design work carried out in producing the application. The folder contains two files:

#### Software Design Document - V1.0.pdf 

A pdf file that contains:
•	Use Case diagram and written use cases capturing user functionality to be provided for by the application.
•	Application architecture description
•	Sequence diagrams for realisation of use cases. 
•	Activity diagram showing the process of importing toy data and updating count, saving changes back to file.
•	Class diagrams for components at each layer of the application. 

#### UML Diagrams - Woorstocks_design_v1.0.uml

A WhiteStarUML project file that contains uml diagrams, class  and sequence diagrams, produced in creating the application.

WhiteStarUML can be found at: http://sourceforge.net/projects/whitestaruml/

#### Testing strategy documentation

A document (Testing_Strategy.pdf) located in \documents\testing strategy provides a description of the testing strategy that was employed in developing the application.