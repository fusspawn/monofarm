import ('Nez.dll', 'Nez')
import ('MonoGame.Framework.dll', 'Microsoft.Xna.Framework')
import ('monotest', 'monotest') 

data = {}
data.pather = nil;
data.target = nil;
data.HARVEST_TYPE = "wood"
data.state = nil;
data.busy = 0;

function OnInit()
	data.start_tile = ChunkManager:WorldToTile( entity.transform.position.X,  entity.transform.position.Y )
	data.mobdata = entity:GetComponentByName("MobData");
	data.inventory = entity:GetComponentByName("Inventory");
	data.pather = entity:GetComponentByName("PathFollower");
	data.state = harvester;
end


function OnUpdate() 
	if(data.busy > 0) then
		data.busy = data.busy - 1;
		return
	end
	if(data.state) then
		data.state()
	end
end


function harvester()
	if (data.pather.HasPath) then
		return
	end

	if (data.inventory:GetItemCount(data.HARVEST_TYPE) > 10) then
		data.pather:SetPath(data.start_tile.X, data.start_tile.Y)
		return;
	end

	if (data.target == nil) then
		find_harvester_target();
		return
	end
	
	data.busy = 100;
	print(tostring(scriptID) .. " want to kill: "  .. tostring(data.target.TileX) .. " / " .. tostring( data.target.TileY ))
	ChunkManager:HarvestResource(data.target.TileX, data.target.TileY, data.HARVEST_TYPE, entity)
	data.inventory:AddItem(itemdefs:GetItemType(data.HARVEST_TYPE), 1)
	data.target = nil	
end


function find_harvester_target()
	data.current_pos = ChunkManager:WorldToTile( entity.transform.position.X,  entity.transform.position.Y )
	data.target = ChunkManager:SearchClosestResource(data.current_pos.X, data.current_pos.Y, data.HARVEST_TYPE)
	if (data.target) then
		data.pather:SetPath(data.target.TileX, data.target.TileY)
	end
end

local ehandler = {}
ehandler["OnInit"] = OnInit
ehandler["OnUpdate"]= OnUpdate

component:RegisterScriptEventHandler(ehandler);