using UnityEngine;

public class MakeupData
{
    public MakeupType makeupType { get; private set; }
    public MakeupTool makeupTool { get; private set; }
    public Sprite makeupSprite { get; private set; }
    public Vector3 toolOffset { get; private set; }
    public Vector3 toolRotation { get; private set; }
    public Vector3 faceOffset { get; private set; }

    public MakeupData(MakeupType makeupType,
                      Sprite makeupSprite,
                      MakeupTool makeupTool,
                      Vector3 toolOffset,
                      Vector3 toolRotation,
                      Vector3 faceOffset)
    {
        this.makeupTool = makeupTool;
        this.makeupSprite = makeupSprite;
        this.makeupType = makeupType;
        this.toolOffset = toolOffset;
        this.toolRotation = toolRotation;
        this.faceOffset = faceOffset;
    }

    public MakeupData(MakeupType makeupType, MakeupTool makeupTool)
    {
        this.makeupTool = makeupTool;
        this.makeupType = makeupType;
    }
}
