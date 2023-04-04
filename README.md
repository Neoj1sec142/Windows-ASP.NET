# Windows-ASP.NET Notes:

- Models, Views, and Controllers. This pattern helps to achieve separation of concerns: 
	* The UI logic belongs in the view. 
	* Input logic belongs in the controller. 
	* Business logic belongs in the model.

- Every public method in a controller is callable as an HTTP endpoint.
- MVC invokes controller classes, and the action methods within them, depending on the incoming URL. The default URL routing logic used by MVC, uses a format like this to determine what code to invoke:
```s
/[Controller]/[ActionName]/[Parameters]
```
- The routing is set in the Program.cs file:
```s
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```