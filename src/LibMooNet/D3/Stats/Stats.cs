// Generated by ProtoGen, Version=2.4.1.473, Culture=neutral, PublicKeyToken=55f7125234beb589.  DO NOT EDIT!
#pragma warning disable 1591, 0612
#region Designer generated code

using pb = global::Google.ProtocolBuffers;
using pbc = global::Google.ProtocolBuffers.Collections;
using pbd = global::Google.ProtocolBuffers.Descriptors;
using scg = global::System.Collections.Generic;
namespace D3.Stats {
  
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ProtoGen", "2.4.1.473")]
  public static partial class Stats {
  
    #region Extension registration
    public static void RegisterAllExtensions(pb::ExtensionRegistry registry) {
    }
    #endregion
    #region Static variables
    internal static pbd::MessageDescriptor internal__static_D3_Stats_StatCategory__Descriptor;
    internal static pb::FieldAccess.FieldAccessorTable<global::D3.Stats.StatCategory, global::D3.Stats.StatCategory.Builder> internal__static_D3_Stats_StatCategory__FieldAccessorTable;
    internal static pbd::MessageDescriptor internal__static_D3_Stats_StatList__Descriptor;
    internal static pb::FieldAccess.FieldAccessorTable<global::D3.Stats.StatList, global::D3.Stats.StatList.Builder> internal__static_D3_Stats_StatList__FieldAccessorTable;
    #endregion
    #region Descriptor
    public static pbd::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbd::FileDescriptor descriptor;
    
    static Stats() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          "CgtTdGF0cy5wcm90bxIIRDMuU3RhdHMiTgoMU3RhdENhdGVnb3J5Eg8KB3N0" + 
          "YXRfaWQYASACKA0SDgoGZGF0YV8xGAIgAigNEg4KBmRhdGFfMhgDIAEoDRIN" + 
          "CgV0b3RhbBgEIAIoBCIxCghTdGF0TGlzdBIlCgVzdGF0cxgBIAMoCzIWLkQz" + 
          "LlN0YXRzLlN0YXRDYXRlZ29yeQ==");
      pbd::FileDescriptor.InternalDescriptorAssigner assigner = delegate(pbd::FileDescriptor root) {
        descriptor = root;
        internal__static_D3_Stats_StatCategory__Descriptor = Descriptor.MessageTypes[0];
        internal__static_D3_Stats_StatCategory__FieldAccessorTable = 
            new pb::FieldAccess.FieldAccessorTable<global::D3.Stats.StatCategory, global::D3.Stats.StatCategory.Builder>(internal__static_D3_Stats_StatCategory__Descriptor,
                new string[] { "StatId", "Data1", "Data2", "Total", });
        internal__static_D3_Stats_StatList__Descriptor = Descriptor.MessageTypes[1];
        internal__static_D3_Stats_StatList__FieldAccessorTable = 
            new pb::FieldAccess.FieldAccessorTable<global::D3.Stats.StatList, global::D3.Stats.StatList.Builder>(internal__static_D3_Stats_StatList__Descriptor,
                new string[] { "Stats", });
        return null;
      };
      pbd::FileDescriptor.InternalBuildGeneratedFileFrom(descriptorData,
          new pbd::FileDescriptor[] {
          }, assigner);
    }
    #endregion
    
  }
  #region Messages
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ProtoGen", "2.4.1.473")]
  public sealed partial class StatCategory : pb::GeneratedMessage<StatCategory, StatCategory.Builder> {
    private StatCategory() { }
    private static readonly StatCategory defaultInstance = new StatCategory().MakeReadOnly();
    private static readonly string[] _statCategoryFieldNames = new string[] { "data_1", "data_2", "stat_id", "total" };
    private static readonly uint[] _statCategoryFieldTags = new uint[] { 16, 24, 8, 32 };
    public static StatCategory DefaultInstance {
      get { return defaultInstance; }
    }
    
    public override StatCategory DefaultInstanceForType {
      get { return DefaultInstance; }
    }
    
    protected override StatCategory ThisMessage {
      get { return this; }
    }
    
    public static pbd::MessageDescriptor Descriptor {
      get { return global::D3.Stats.Stats.internal__static_D3_Stats_StatCategory__Descriptor; }
    }
    
    protected override pb::FieldAccess.FieldAccessorTable<StatCategory, StatCategory.Builder> InternalFieldAccessors {
      get { return global::D3.Stats.Stats.internal__static_D3_Stats_StatCategory__FieldAccessorTable; }
    }
    
    public const int StatIdFieldNumber = 1;
    private bool hasStatId;
    private uint statId_;
    public bool HasStatId {
      get { return hasStatId; }
    }
    public uint StatId {
      get { return statId_; }
    }
    
    public const int Data1FieldNumber = 2;
    private bool hasData1;
    private uint data1_;
    public bool HasData1 {
      get { return hasData1; }
    }
    public uint Data1 {
      get { return data1_; }
    }
    
    public const int Data2FieldNumber = 3;
    private bool hasData2;
    private uint data2_;
    public bool HasData2 {
      get { return hasData2; }
    }
    public uint Data2 {
      get { return data2_; }
    }
    
    public const int TotalFieldNumber = 4;
    private bool hasTotal;
    private ulong total_;
    public bool HasTotal {
      get { return hasTotal; }
    }
    public ulong Total {
      get { return total_; }
    }
    
    public override bool IsInitialized {
      get {
        if (!hasStatId) return false;
        if (!hasData1) return false;
        if (!hasTotal) return false;
        return true;
      }
    }
    
    public override void WriteTo(pb::ICodedOutputStream output) {
      int size = SerializedSize;
      string[] field_names = _statCategoryFieldNames;
      if (hasStatId) {
        output.WriteUInt32(1, field_names[2], StatId);
      }
      if (hasData1) {
        output.WriteUInt32(2, field_names[0], Data1);
      }
      if (hasData2) {
        output.WriteUInt32(3, field_names[1], Data2);
      }
      if (hasTotal) {
        output.WriteUInt64(4, field_names[3], Total);
      }
      UnknownFields.WriteTo(output);
    }
    
    private int memoizedSerializedSize = -1;
    public override int SerializedSize {
      get {
        int size = memoizedSerializedSize;
        if (size != -1) return size;
        
        size = 0;
        if (hasStatId) {
          size += pb::CodedOutputStream.ComputeUInt32Size(1, StatId);
        }
        if (hasData1) {
          size += pb::CodedOutputStream.ComputeUInt32Size(2, Data1);
        }
        if (hasData2) {
          size += pb::CodedOutputStream.ComputeUInt32Size(3, Data2);
        }
        if (hasTotal) {
          size += pb::CodedOutputStream.ComputeUInt64Size(4, Total);
        }
        size += UnknownFields.SerializedSize;
        memoizedSerializedSize = size;
        return size;
      }
    }
    
    public static StatCategory ParseFrom(pb::ByteString data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static StatCategory ParseFrom(pb::ByteString data, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
    }
    public static StatCategory ParseFrom(byte[] data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static StatCategory ParseFrom(byte[] data, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
    }
    public static StatCategory ParseFrom(global::System.IO.Stream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static StatCategory ParseFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
    }
    public static StatCategory ParseDelimitedFrom(global::System.IO.Stream input) {
      return CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
    }
    public static StatCategory ParseDelimitedFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
      return CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
    }
    public static StatCategory ParseFrom(pb::ICodedInputStream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static StatCategory ParseFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
    }
    private StatCategory MakeReadOnly() {
      return this;
    }
    
    public static Builder CreateBuilder() { return new Builder(); }
    public override Builder ToBuilder() { return CreateBuilder(this); }
    public override Builder CreateBuilderForType() { return new Builder(); }
    public static Builder CreateBuilder(StatCategory prototype) {
      return new Builder(prototype);
    }
    
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ProtoGen", "2.4.1.473")]
    public sealed partial class Builder : pb::GeneratedBuilder<StatCategory, Builder> {
      protected override Builder ThisBuilder {
        get { return this; }
      }
      public Builder() {
        result = DefaultInstance;
        resultIsReadOnly = true;
      }
      internal Builder(StatCategory cloneFrom) {
        result = cloneFrom;
        resultIsReadOnly = true;
      }
      
      private bool resultIsReadOnly;
      private StatCategory result;
      
      private StatCategory PrepareBuilder() {
        if (resultIsReadOnly) {
          StatCategory original = result;
          result = new StatCategory();
          resultIsReadOnly = false;
          MergeFrom(original);
        }
        return result;
      }
      
      public override bool IsInitialized {
        get { return result.IsInitialized; }
      }
      
      protected override StatCategory MessageBeingBuilt {
        get { return PrepareBuilder(); }
      }
      
      public override Builder Clear() {
        result = DefaultInstance;
        resultIsReadOnly = true;
        return this;
      }
      
      public override Builder Clone() {
        if (resultIsReadOnly) {
          return new Builder(result);
        } else {
          return new Builder().MergeFrom(result);
        }
      }
      
      public override pbd::MessageDescriptor DescriptorForType {
        get { return global::D3.Stats.StatCategory.Descriptor; }
      }
      
      public override StatCategory DefaultInstanceForType {
        get { return global::D3.Stats.StatCategory.DefaultInstance; }
      }
      
      public override StatCategory BuildPartial() {
        if (resultIsReadOnly) {
          return result;
        }
        resultIsReadOnly = true;
        return result.MakeReadOnly();
      }
      
      public override Builder MergeFrom(pb::IMessage other) {
        if (other is StatCategory) {
          return MergeFrom((StatCategory) other);
        } else {
          base.MergeFrom(other);
          return this;
        }
      }
      
      public override Builder MergeFrom(StatCategory other) {
        if (other == global::D3.Stats.StatCategory.DefaultInstance) return this;
        PrepareBuilder();
        if (other.HasStatId) {
          StatId = other.StatId;
        }
        if (other.HasData1) {
          Data1 = other.Data1;
        }
        if (other.HasData2) {
          Data2 = other.Data2;
        }
        if (other.HasTotal) {
          Total = other.Total;
        }
        this.MergeUnknownFields(other.UnknownFields);
        return this;
      }
      
      public override Builder MergeFrom(pb::ICodedInputStream input) {
        return MergeFrom(input, pb::ExtensionRegistry.Empty);
      }
      
      public override Builder MergeFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
        PrepareBuilder();
        pb::UnknownFieldSet.Builder unknownFields = null;
        uint tag;
        string field_name;
        while (input.ReadTag(out tag, out field_name)) {
          if(tag == 0 && field_name != null) {
            int field_ordinal = global::System.Array.BinarySearch(_statCategoryFieldNames, field_name, global::System.StringComparer.Ordinal);
            if(field_ordinal >= 0)
              tag = _statCategoryFieldTags[field_ordinal];
            else {
              if (unknownFields == null) {
                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
              }
              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
              continue;
            }
          }
          switch (tag) {
            case 0: {
              throw pb::InvalidProtocolBufferException.InvalidTag();
            }
            default: {
              if (pb::WireFormat.IsEndGroupTag(tag)) {
                if (unknownFields != null) {
                  this.UnknownFields = unknownFields.Build();
                }
                return this;
              }
              if (unknownFields == null) {
                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
              }
              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
              break;
            }
            case 8: {
              result.hasStatId = input.ReadUInt32(ref result.statId_);
              break;
            }
            case 16: {
              result.hasData1 = input.ReadUInt32(ref result.data1_);
              break;
            }
            case 24: {
              result.hasData2 = input.ReadUInt32(ref result.data2_);
              break;
            }
            case 32: {
              result.hasTotal = input.ReadUInt64(ref result.total_);
              break;
            }
          }
        }
        
        if (unknownFields != null) {
          this.UnknownFields = unknownFields.Build();
        }
        return this;
      }
      
      
      public bool HasStatId {
        get { return result.hasStatId; }
      }
      public uint StatId {
        get { return result.StatId; }
        set { SetStatId(value); }
      }
      public Builder SetStatId(uint value) {
        PrepareBuilder();
        result.hasStatId = true;
        result.statId_ = value;
        return this;
      }
      public Builder ClearStatId() {
        PrepareBuilder();
        result.hasStatId = false;
        result.statId_ = 0;
        return this;
      }
      
      public bool HasData1 {
        get { return result.hasData1; }
      }
      public uint Data1 {
        get { return result.Data1; }
        set { SetData1(value); }
      }
      public Builder SetData1(uint value) {
        PrepareBuilder();
        result.hasData1 = true;
        result.data1_ = value;
        return this;
      }
      public Builder ClearData1() {
        PrepareBuilder();
        result.hasData1 = false;
        result.data1_ = 0;
        return this;
      }
      
      public bool HasData2 {
        get { return result.hasData2; }
      }
      public uint Data2 {
        get { return result.Data2; }
        set { SetData2(value); }
      }
      public Builder SetData2(uint value) {
        PrepareBuilder();
        result.hasData2 = true;
        result.data2_ = value;
        return this;
      }
      public Builder ClearData2() {
        PrepareBuilder();
        result.hasData2 = false;
        result.data2_ = 0;
        return this;
      }
      
      public bool HasTotal {
        get { return result.hasTotal; }
      }
      public ulong Total {
        get { return result.Total; }
        set { SetTotal(value); }
      }
      public Builder SetTotal(ulong value) {
        PrepareBuilder();
        result.hasTotal = true;
        result.total_ = value;
        return this;
      }
      public Builder ClearTotal() {
        PrepareBuilder();
        result.hasTotal = false;
        result.total_ = 0UL;
        return this;
      }
    }
    static StatCategory() {
      object.ReferenceEquals(global::D3.Stats.Stats.Descriptor, null);
    }
  }
  
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ProtoGen", "2.4.1.473")]
  public sealed partial class StatList : pb::GeneratedMessage<StatList, StatList.Builder> {
    private StatList() { }
    private static readonly StatList defaultInstance = new StatList().MakeReadOnly();
    private static readonly string[] _statListFieldNames = new string[] { "stats" };
    private static readonly uint[] _statListFieldTags = new uint[] { 10 };
    public static StatList DefaultInstance {
      get { return defaultInstance; }
    }
    
    public override StatList DefaultInstanceForType {
      get { return DefaultInstance; }
    }
    
    protected override StatList ThisMessage {
      get { return this; }
    }
    
    public static pbd::MessageDescriptor Descriptor {
      get { return global::D3.Stats.Stats.internal__static_D3_Stats_StatList__Descriptor; }
    }
    
    protected override pb::FieldAccess.FieldAccessorTable<StatList, StatList.Builder> InternalFieldAccessors {
      get { return global::D3.Stats.Stats.internal__static_D3_Stats_StatList__FieldAccessorTable; }
    }
    
    public const int StatsFieldNumber = 1;
    private pbc::PopsicleList<global::D3.Stats.StatCategory> stats_ = new pbc::PopsicleList<global::D3.Stats.StatCategory>();
    public scg::IList<global::D3.Stats.StatCategory> StatsList {
      get { return stats_; }
    }
    public int StatsCount {
      get { return stats_.Count; }
    }
    public global::D3.Stats.StatCategory GetStats(int index) {
      return stats_[index];
    }
    
    public override bool IsInitialized {
      get {
        foreach (global::D3.Stats.StatCategory element in StatsList) {
          if (!element.IsInitialized) return false;
        }
        return true;
      }
    }
    
    public override void WriteTo(pb::ICodedOutputStream output) {
      int size = SerializedSize;
      string[] field_names = _statListFieldNames;
      if (stats_.Count > 0) {
        output.WriteMessageArray(1, field_names[0], stats_);
      }
      UnknownFields.WriteTo(output);
    }
    
    private int memoizedSerializedSize = -1;
    public override int SerializedSize {
      get {
        int size = memoizedSerializedSize;
        if (size != -1) return size;
        
        size = 0;
        foreach (global::D3.Stats.StatCategory element in StatsList) {
          size += pb::CodedOutputStream.ComputeMessageSize(1, element);
        }
        size += UnknownFields.SerializedSize;
        memoizedSerializedSize = size;
        return size;
      }
    }
    
    public static StatList ParseFrom(pb::ByteString data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static StatList ParseFrom(pb::ByteString data, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
    }
    public static StatList ParseFrom(byte[] data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static StatList ParseFrom(byte[] data, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
    }
    public static StatList ParseFrom(global::System.IO.Stream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static StatList ParseFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
    }
    public static StatList ParseDelimitedFrom(global::System.IO.Stream input) {
      return CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
    }
    public static StatList ParseDelimitedFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
      return CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
    }
    public static StatList ParseFrom(pb::ICodedInputStream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static StatList ParseFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
    }
    private StatList MakeReadOnly() {
      stats_.MakeReadOnly();
      return this;
    }
    
    public static Builder CreateBuilder() { return new Builder(); }
    public override Builder ToBuilder() { return CreateBuilder(this); }
    public override Builder CreateBuilderForType() { return new Builder(); }
    public static Builder CreateBuilder(StatList prototype) {
      return new Builder(prototype);
    }
    
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ProtoGen", "2.4.1.473")]
    public sealed partial class Builder : pb::GeneratedBuilder<StatList, Builder> {
      protected override Builder ThisBuilder {
        get { return this; }
      }
      public Builder() {
        result = DefaultInstance;
        resultIsReadOnly = true;
      }
      internal Builder(StatList cloneFrom) {
        result = cloneFrom;
        resultIsReadOnly = true;
      }
      
      private bool resultIsReadOnly;
      private StatList result;
      
      private StatList PrepareBuilder() {
        if (resultIsReadOnly) {
          StatList original = result;
          result = new StatList();
          resultIsReadOnly = false;
          MergeFrom(original);
        }
        return result;
      }
      
      public override bool IsInitialized {
        get { return result.IsInitialized; }
      }
      
      protected override StatList MessageBeingBuilt {
        get { return PrepareBuilder(); }
      }
      
      public override Builder Clear() {
        result = DefaultInstance;
        resultIsReadOnly = true;
        return this;
      }
      
      public override Builder Clone() {
        if (resultIsReadOnly) {
          return new Builder(result);
        } else {
          return new Builder().MergeFrom(result);
        }
      }
      
      public override pbd::MessageDescriptor DescriptorForType {
        get { return global::D3.Stats.StatList.Descriptor; }
      }
      
      public override StatList DefaultInstanceForType {
        get { return global::D3.Stats.StatList.DefaultInstance; }
      }
      
      public override StatList BuildPartial() {
        if (resultIsReadOnly) {
          return result;
        }
        resultIsReadOnly = true;
        return result.MakeReadOnly();
      }
      
      public override Builder MergeFrom(pb::IMessage other) {
        if (other is StatList) {
          return MergeFrom((StatList) other);
        } else {
          base.MergeFrom(other);
          return this;
        }
      }
      
      public override Builder MergeFrom(StatList other) {
        if (other == global::D3.Stats.StatList.DefaultInstance) return this;
        PrepareBuilder();
        if (other.stats_.Count != 0) {
          result.stats_.Add(other.stats_);
        }
        this.MergeUnknownFields(other.UnknownFields);
        return this;
      }
      
      public override Builder MergeFrom(pb::ICodedInputStream input) {
        return MergeFrom(input, pb::ExtensionRegistry.Empty);
      }
      
      public override Builder MergeFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
        PrepareBuilder();
        pb::UnknownFieldSet.Builder unknownFields = null;
        uint tag;
        string field_name;
        while (input.ReadTag(out tag, out field_name)) {
          if(tag == 0 && field_name != null) {
            int field_ordinal = global::System.Array.BinarySearch(_statListFieldNames, field_name, global::System.StringComparer.Ordinal);
            if(field_ordinal >= 0)
              tag = _statListFieldTags[field_ordinal];
            else {
              if (unknownFields == null) {
                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
              }
              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
              continue;
            }
          }
          switch (tag) {
            case 0: {
              throw pb::InvalidProtocolBufferException.InvalidTag();
            }
            default: {
              if (pb::WireFormat.IsEndGroupTag(tag)) {
                if (unknownFields != null) {
                  this.UnknownFields = unknownFields.Build();
                }
                return this;
              }
              if (unknownFields == null) {
                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
              }
              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
              break;
            }
            case 10: {
              input.ReadMessageArray(tag, field_name, result.stats_, global::D3.Stats.StatCategory.DefaultInstance, extensionRegistry);
              break;
            }
          }
        }
        
        if (unknownFields != null) {
          this.UnknownFields = unknownFields.Build();
        }
        return this;
      }
      
      
      public pbc::IPopsicleList<global::D3.Stats.StatCategory> StatsList {
        get { return PrepareBuilder().stats_; }
      }
      public int StatsCount {
        get { return result.StatsCount; }
      }
      public global::D3.Stats.StatCategory GetStats(int index) {
        return result.GetStats(index);
      }
      public Builder SetStats(int index, global::D3.Stats.StatCategory value) {
        pb::ThrowHelper.ThrowIfNull(value, "value");
        PrepareBuilder();
        result.stats_[index] = value;
        return this;
      }
      public Builder SetStats(int index, global::D3.Stats.StatCategory.Builder builderForValue) {
        pb::ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
        PrepareBuilder();
        result.stats_[index] = builderForValue.Build();
        return this;
      }
      public Builder AddStats(global::D3.Stats.StatCategory value) {
        pb::ThrowHelper.ThrowIfNull(value, "value");
        PrepareBuilder();
        result.stats_.Add(value);
        return this;
      }
      public Builder AddStats(global::D3.Stats.StatCategory.Builder builderForValue) {
        pb::ThrowHelper.ThrowIfNull(builderForValue, "builderForValue");
        PrepareBuilder();
        result.stats_.Add(builderForValue.Build());
        return this;
      }
      public Builder AddRangeStats(scg::IEnumerable<global::D3.Stats.StatCategory> values) {
        PrepareBuilder();
        result.stats_.Add(values);
        return this;
      }
      public Builder ClearStats() {
        PrepareBuilder();
        result.stats_.Clear();
        return this;
      }
    }
    static StatList() {
      object.ReferenceEquals(global::D3.Stats.Stats.Descriptor, null);
    }
  }
  
  #endregion
  
}

#endregion Designer generated code