# Essence IoC

[![AppVeyor Build status](https://ci.appveyor.com/api/projects/status/github/tkratena/essence-ioc?branch=master&svg=true)](https://ci.appveyor.com/project/tkratena/essence-ioc?branch=master) [![NuGet Version and Downloads count](https://buildstats.info/nuget/Essence.Ioc.FluentRegistration)](https://www.nuget.org/packages/Essence.Ioc.FluentRegistration)

"From injection to design"

Lightweight [IoC container](https://www.martinfowler.com/articles/injection.html) that strives to motivate its users to design their code well instead of offering a quantity of features that could hide design flaws.

## Installation

Use the [nuget package](https://www.nuget.org/packages/Essence.Ioc.FluentRegistration) to install Essence IoC with fluent registration.

## Usage

```cs
class MyApplication : IDisposable
{
  private readonly Essence.Ioc.Container _container;
  
  public MyApplication()
  {
    _container = new Essence.Ioc.Container(r =>
    {
      r.RegisterService<IDependency>().ImplementedBy<DependencyImplementation>();
      r.RegisterService<IService>().ImplementedBy<ServiceImplementation>();
    });
  }
  
  public void ExecuteMyUseCase()
  {
    using (_container.Resolve<IService>(out var service))
    {
      service.Use();
    }
  }
  
  public void Dispose()
  {
    _container.Dispose();
  }
}
```

### Constructor injection
Only constructor injection is supported.
```cs
class ServiceImplementation : IService
{
  private readonly IDependency _dependency;
  
  public ServiceImplementation(IDependency dependency)
  {
    _dependency = dependency;
  }
  
  public void Use()
  {
    _dependency.Use();
  }
}
```

### Registration order
When registering a service, dependencies of its implementation must be already registered. That means registrations must be ordered from independent services to the ones that have more and more dependencies.
```cs
new Essence.Ioc.Container(r =>
{
  r.RegisterService<IElectricity>().ImplementedBy<Electricity>();
  r.RegisterService<IDoor>().ImplementedBy<ElectricDoor>(); // depends on IElectricity
  r.RegisterService<IWater>().ImplementedBy<Water>();
  r.RegisterService<IWashDevice>().ImplementedBy<RotatingBrushes>(); // depends on IElectricity and IWater
  r.RegisterService<ICarWash>().ImplementedBy<CarWash>(); // depends on IDoor and IWashingDevice
})
```

### Injection types
Not only an instance of the dependency but also a lazy instance and an instance factory delegate can be injected.
```cs
class ServiceImplementation : IService
{
  public ServiceImplementation(
    IDependency dependency, 
    Lazy<IDependency> lazyDependency, 
    Func<IDependency> dependencyFactory)
  {
  }
}
```

### Life cycle management
`IDisposable` service implementations are disposed by the container at the end of their life scope, taking their life style and injection context into account.

Service interfaces must not be `IDisposable` as life cycle should be an implementation detail and should be managed by the container.

### Reference documentation
See [unit tests](https://github.com/tkratena/essence-ioc/tree/master/EssenceIoc/Essence.Ioc.UnitTests) to find more examples and details about Essence IoC features.

## Changelog

### [2.0.0] - 2020-02-23
#### Added
- [Life cycle management](https://github.com/tkratena/essence-ioc/issues/38) of disposable service implementations.
- Adhering to [semantic versioning](https://github.com/tkratena/essence-ioc/issues/49) from now on.
#### Changed
- Resolved instance is returned via out parameter of `Resolve()` method. The method returns an `IDisposable` to be able to end the transient [life scope](https://github.com/tkratena/essence-ioc/issues/38).
- The container implements `IDisposable` to be able to end the singleton [life scope](https://github.com/tkratena/essence-ioc/issues/38).
- Registering after container is constructed [fails fast](https://github.com/tkratena/essence-ioc/issues/44).

## License
[MIT](https://github.com/tkratena/essence-ioc/blob/release/2.0/LICENSE)
