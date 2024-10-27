using CommonLib.NamedValues;

namespace SF3.Values {
    public class FriendshipBonusTypeValueResourceInfo : NamedValueFromResourceInfo {
        public FriendshipBonusTypeValueResourceInfo() : base("FriendshipBonusTypeList.xml") {
        }
    }

    /// <summary>
    /// Named value for friendship bonus type that can be bound to an ObjectListView.
    /// </summary>
    public class FriendshipBonusTypeValue : NamedValueFromResource<FriendshipBonusTypeValueResourceInfo> {
        public FriendshipBonusTypeValue(int value) : base(value) {
        }

        public override NamedValue MakeRelatedValue(int value) => new FriendshipBonusTypeValue(value);
    }
}
