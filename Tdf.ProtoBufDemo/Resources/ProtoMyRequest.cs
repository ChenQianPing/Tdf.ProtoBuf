//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: ProtoMyRequest.proto
namespace MyProtoBuf
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MyRequest")]
  public partial class MyRequest : global::ProtoBuf.IExtensible
  {
    public MyRequest() {}
    
    private int _version;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"version", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int version
    {
      get { return _version; }
      set { _version = value; }
    }
    private string _name;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }

    private string _website = @"http://www.apache.org/";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"website", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(@"http://www.apache.org/")]
    public string website
    {
      get { return _website; }
      set { _website = value; }
    }

    private byte[] _data = null;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public byte[] data
    {
      get { return _data; }
      set { _data = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}