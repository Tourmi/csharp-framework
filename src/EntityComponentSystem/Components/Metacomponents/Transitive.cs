namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// Marks a relation as transitive.
/// ie: If <c>Car.IsA(MotorVehicule)</c> and <c>MotorVehicule.IsA(Vehicule)</c> are both <see langword="true"/>, 
/// then <c>Car.IsA(Vehicule)</c> will be <see langword="true"/> too.
/// </summary>
[TagComponent]
public readonly struct Transitive;
