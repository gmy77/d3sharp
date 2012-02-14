Field names in proto definitions may collide with default properties for objects in C#. 
This causes ProtoBuffer to crash because of ambigious names.

An example is QuestReward, which has a field 'Item' that collides with the hidden (in c#) property
item for collections that is generated for .net languages that dont support indexers...

Easiest way to fix it is to just rename these fields.