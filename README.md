.Net Sample + Toolkit
=======================

<b>Note:</b> For using those samples you need a valid oAuth credential and a ReCap client ID. Visit this [page](http://developer-recap-autodesk.github.io/) for instructions to get on-board.


Dependencies
--------------------
This sample is dependent of following 3rd party assemblies:

1. The RestSharp assembly

     RestSharp - Simple REST and HTTP Client for .NET
	 you need at least version 104.3.3. You can get this component source code [here](http://restsharp.org/)

2. The Xceed WPF Toolkit Community Edition

     this assembly is only used if you want to display the ReCap properties in a Property window. Properties are anyway dumped into a text window. 
	 You can get the binaries and documentation from [https://wpftoolkit.codeplex.com/](https://wpftoolkit.codeplex.com/)


Building the sample
---------------------------

The sample was created using Visual Studio 2012 Service Pack 1

You first need to modify the UserSettings.cs file and put your oAuth & ReCap credentials in it.
	 
Use of the sample
-------------------------

* When you launch the sample, the application will try to connect to the ReCap server and verifies that you are properly authorized on the Autodesk oAuth server. 
If you are, it will refresh your access token immediately. If not, it will ask you to get authorized. 

* The samples display a treeview that contains existing ReCap photoscenes, right-click on the root node to display a context menu that allows to create a new scene.
Right-click on each scene node to display options.


--------

## License

This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT). Please see the [LICENSE](LICENSE) file for full details.


## Written by

Philippe Leefsma (Autodesk Developer Network)  
http://www.autodesk.com/adn  
