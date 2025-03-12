Hunger Utils
=================

A server-side mod that adds a command which can get/set certain Hunger attributes on players. 

`/hungerutils <playername> <attribute> [<setvalue>]`

`[<setvalue>]` is optional, and the command will return the current value of the attribute if the set value is not specified.

Attributes (case-sensitive) that are settable and gettable are:

* Saturation
* SaturationLossDelayFruit
* SaturationLossDelayVegetable
* SaturationLossDelayProtein
* SaturationLossDelayGrain
* SaturationLossDelayDairy
* MaxSaturation
* FruitLevel
* VegetableLevel
* ProteinLevel
* GrainLevel
* DairyLevel

This should help with debugging issues like frozen satiety bars on servers.

