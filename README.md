# CPlusPlus
 
This is the full C# engine that I planned to recreate in C++.

I attempted to handle data managment by creating a Cache class that stores all game related stuff, such as loaded meshes, textures and shaders.
I think during a rework, the Cache class duplicated and now contains code from the previous version of the Cache class, and the reworked class.

I have reworked alot of the code in class to be more functional and less hard coded. How the window renders and updates the scene, how scenes are managed, editors are used.
Despite that, there is still alot of code that exists from the initial testing phase. I havent tested it, but I have a feeling that there are some errors in my cache system that may not account for 
	certain meshes/textures/shaders being created, which may lead to memory leaks, and multiple instances of the same mesh/texture/shader being created. This needs to be fixed.
	
One of the things I was last working on was making it possible for an entity to move forwards no matter their rotation. I've tried multiple different solutions and had semi decent results, but nothing 100%.
	I currently am now using a Quaternion class to handle rotations, instead of using angles to rotate and object, but I have yet to properly work into the final transformation_matrix.
	
Working on this engine has given me alot of insight on how engines work themselves, as well as how I should store data.

I never planned to show this project off at this stage so there aren't really any comments to speak of, maybe todo comments to remind me what I was doing last. But I tried to make things more readable as I was redoing the code
	and to reduce the amount fo duplicate code.