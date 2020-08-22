local text = 'placeholder';

ImGui:Begin('test')
ImGui:InputText('hey, enter something!', text, 64)
if ImGui:Button('woah!!') then
    print('you pressed woah and you entered: '..text)
end
ImGui:End()