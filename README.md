# Theo's panel 0.1

## What is Theo's panel?

Theo's panel is a free server/database management tool for Arma 3 Altis Life. It allows staff to manage the altis life database as well as providing other features such as support cases.

## What features does this panel currently have?


* Ability to view and edit player stats such as cash, faction levels etc
* An easy to use player license management tool
* Ability to view and edit vehicle
* Ability to view and edit houses
* Ability to view and edit gangs
* A fully featured permission system allowing custom staff ranks with full control of what permissions they have
* Easy to use UI
* Support cases
* Dashboard containing server stats such as total players
* Many more features to come!

## Requirements

* Windows server
* IIS

This panel has been written using ASP.NET MVC and has not been tested on Apache/Linux, however it may be possible using Mono.

## Installation

I reccommend using IIS to run this site, to install IIS find the tutorial relevant to your version of windows server. Such as the following https://docs.microsoft.com/en-us/iis/get-started/whats-new-in-iis-8/installing-iis-8-on-windows-server-2012

Once IIS has been installed you need to add a new website, the compiled version of the panel can be found in bin/Release/ if you don't have Visual Studio. I also reccommend following a tutorial for this as well as it can be confusing, find the relevant tutorial for your version of Windows Server.

Once IIS has been installed and you have the web site added visit the URL in the panel and naviagte to /Setup this will run you through setting the database parameters as well as creating a root account

NOTE - Currently the community name will not be set after setup this will be fixed soon however you can manually set it in the Web.Config file and the appsettings section.
## Updating

Updating is as simple as getting the latest compiled files and replacing them.
