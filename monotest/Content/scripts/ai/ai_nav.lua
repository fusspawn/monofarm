data.path_target = nil;
data.next_location = nil;
data.current_path = nil;
data.current_path_pos = nil;
data.on_nav_complete = nil;

function OnPathUpdate()
	if(data.current_path != nil or data.current_path.Count == 0) then
		if(data.path_target != nil) then
			data.current_path = NavigationSystem.FindPath(data.path_target.X, data.path_target.Y)
			data.next_location = data.current_path:Dequeue()
			return;
		end
	end

	data.current_path_pos = ChunkManager:WorldToTile( entity.transform.position.X,  entity.transform.position.Y )
	if(data.current_path_pos.X == data.next_location.X and data.current_path_pos.Y == data.next_location.Y) then
		if(data.current_path.Count == 0) then
			data.current_path = nil
			data.path_target = nil

			if(data.on_nav_complete) then
				data.on_nav_complete()
			end
			return
		end

		data.next_location = data.current_path:Dequeue()
		return;
	end

	entity.transform.position = Vector2.Lerp(entity.transform.position, Vector2(data.path_target.X, data.path_target.Y))
end


function ai_has_path()
	return data.current_path.Count > 0;
end

