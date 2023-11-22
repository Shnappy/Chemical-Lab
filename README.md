
# Chemical Lab

Welcome to ***Chemical Lab*** repo!  

This is a tiny chem lab simulation project which uses Unity game engine to visualize various reactions between
various chemical ingredients.

>Important! All the colors of chemical compositions are made-up and only serve for the purpose of demonstration

### Features

1. Physical simulation of chemical compositions

![](/Screenshots/1.jpg)
>default lab set-up

Simulation is being done with the us of **Liquid** and **ChemicalComponent** classes. 

**Liquid** class describes the physical behaviour of a liquid within a container. To make a more realistic liquid of a certain
liquid such parameters as thickness, max wobble and wobble speed move can be changed for a specific chemical element/composition.

**ChemicalComponent** class describes the chemical property of the object. It contains the chemical notation for the chemical element
contained aswell as it's reaction to various indicators.

2. Chemical reactions

![](/Screenshots/2.jpg)
>Result of adding O2 from the first testtube to the rest of testtubes

When chemical elements/compositions interract within a vessel, such interraction may lead to a reaction resulting two liquids in merging into a new
composition. If reaction didn't occur, liquids will stack upon each other. It is currently set that the last added liquid will settle
on top. Liquid stacking is not visual at this moment and the newly added liquid is instatiated outside of lab, but all of the class
properties assigned to it are preserved, and hence it can be used for further interactions.

All of chemical reactions are described in **ChemicalReactions** class. Currently, for the purpose of demo it contains just one
chemical reaction, but it should be trivial to add more.

3. Indicators

![](/Screenshots/3.jpg)
>Result of applying phenolphthalein to all of the testtubes

Every single chemical component contains a reaction to the indicators that are set on the right side of the lab. Adding an indicator
will change the color of the liquid stored within a vessel.

### TODO

1. Add visualization of liquid stacking.
2. Add proper chemical reactions (might need a chemical formulae calcualator implementation).
3. Add gaseous products from reactions.
