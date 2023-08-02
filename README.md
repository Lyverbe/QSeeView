# QSeeView
Manager for QSee devices.

I've wanted something to easily download batch of videos off of my QCW4 device.  The existing QCView application is not made for this and using other software is a pain.  Since I couldn't find any, I made one.  Gradually, I added more features, more and more.  I ended up recreating QCView... but better :)

- The DLLs inside the "_SDK/libs" folder need to be at the same place as the executable.
- To convert DAV files to AVI, FFMPEG needs to be installed and its path needs to be specified in the application settings.
- Command line argument "-live" can be used to start the application with the live view
