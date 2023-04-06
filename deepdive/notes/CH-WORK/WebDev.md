# Intro to Web Development w/ ASP.NET Core

- ASP.NET MVC was released in 2009 to cleanly separate the concerns of web developers between
the **models**, which temporarily store the data; the **views**, which present the data using various formats in the UI; and the **controllers**, which fetch the model and pass it to a view. This separation enables improved reuse and unit testing.

- You can develop and run ASP.NET Core applications cross-platform on Windows, macOS, and Linux. 
Microsoft has even created a cross-platform, super-performant web server named Kestrel, and the
entire stack is open source

- ASP.NET Core provides multiple technologies for building websites:
    • ASP.NET Core Razor Pages and Razor class libraries are ways to dynamically generate HTML 
    for simple websites. You will learn about them in detail in Chapter 13, Building Websites Using 
    ASP.NET Core Razor Pages.
    • ASP.NET Core MVC is an implementation of the Model-View-Controller (MVC) design pattern 
    that is popular for developing complex websites. You will learn about it in detail in Chapter 14, 
    Building Websites Using the Model-View-Controller Pattern.
    • Blazor lets you build user interface components using C# and .NET instead of a JavaScript-based 
    UI framework like Angular, React, and Vue. Blazor WebAssembly runs your code in the browser like a JavaScript-based framework would. Blazor Server runs your code on the server and 
    updates the web page dynamically. You will learn about Blazor in detail in Chapter 16, Building 
    User Interfaces Using Blazor. Blazor is not just for building websites; it can also be used to create 
    hybrid mobile and desktop apps by being hosted inside a .NET MAUI app