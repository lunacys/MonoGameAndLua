local text = 'placeholder1';
local myImage = ImageLoader:Load('test.png');
local myImageBinding = ImGuiTexture:BindTexture(myImage);
local imageSize = { myImage.Width, myImage.Height };
local result = {}
--setmetatable(result, result)

print('hey12');

function Render()
    if Input:IsKeyDown(Keys.Space) then
        print('space is down!')
    end
    ImGui:Begin('test')
    
    local _, newText = ImGui:InputText('hey, enter something!', text, 64, 32)
    text = newText

    if ImGui:Button('woah!!') then
        print('you pressed woah and you entered: '..text)
    end
    ImGui:SameLine()
    if ImGui:Button('woah #2') then
        print('you pressed woah #2!!!: '..newText)
    end
    ImGui:SameLine()
    if ImGui:Button('woah #3') then
        print('you pressed woah #3!!!: '..newText)
    end
    ImGui:SameLine()
    if ImGui:Button('woah #4') then
        print('you pressed woah #4!!!!!: '..newText)
    end
    ImGui:SameLine()
    if ImGui:Button('woah #5') then
        print('you pressed woah #5!!!!!: '..newText)
    end
    ImGui:Text('my text label!')
    ImGui:Text('my second text label!')

    ImGui:Image(myImageBinding, imageSize)

    ImGui:End()
end

function result:test()
    print('test')
end

function result:render()
    ImGui:Begin('result:render')
    ImGui:Button('dis is test render')
    ImGui:End()
end

return result